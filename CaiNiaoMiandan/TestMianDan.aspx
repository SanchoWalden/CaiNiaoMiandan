<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMianDan.aspx.cs" Inherits="CaiNiaoMiandan.TestMianDan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <script type="text/javascript">
        var webSocket;
        var defaultPrinter;

        //面单号
        var waybillNO = '9890000076011';
        //模板url
        var waybillTemplateURL = 'http://cloudprint.cainiao.com/cloudprint/template/getStandardTemplate.json?template_id=801';
        //自定义模板url
        var customAreaURL = 'http://cloudprint.cainiao.com/cloudprint/customArea/queryCustomAreaList4Top.json?custom_area_id=2201&user_id=2066393830';

        //备注：webSocket 是全局对象，不要每次发送请求丢去创建一个，做到webSocket对象重用，和打印组件保持长连接。(1、连接打印客户端)
        function doConnect() {
            socket = new WebSocket('ws://localhost:13528');
            //如果是https的话，端口是13529
            //socket = new WebSocket('wss://localhost:13529');
            // 打开Socket
            socket.onopen = function (event) {
                // 监听消息
                socket.onmessage = function (event) {
                    console.log('Client received a message', event);
                    var data = JSON.parse(event.data);
                    if ("getPrinters" == data.cmd) {
                        alert('打印机列表:' + JSON.stringify(data.printers));
                        defaultPrinter = data.defaultPrinter;
                        alert('默认打印机为:' + defaultPrinter);
                    } else {
                        alert("返回数据:" + JSON.stringify(data));
                    }
                };
                // 监听Socket的关闭
                socket.onclose = function (event) {
                    console.log('Client notified socket has closed', event);
                };
                socket.onerror = function (event) {
                    alert('无法连接到:' + printer_address);
                };
            };
        }
        /***
         * 
         * 获取请求的UUID，指定长度和进制,如 
         * getUUID(8, 2)   //"01001010" 8 character (base=2)
         * getUUID(8, 10) // "47473046" 8 character ID (base=10)
         * getUUID(8, 16) // "098F4D35"。 8 character ID (base=16)
         *   
         */
        function getUUID(len, radix) {
            var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
            var uuid = [], i;
            radix = radix || chars.length;
            if (len) {
                for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
            } else {
                var r;
                uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
                uuid[14] = '4';
                for (i = 0; i < 36; i++) {
                    if (!uuid[i]) {
                        r = 0 | Math.random() * 16;
                        uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
                    }
                }
            }
            return uuid.join('');
        }
        /***
         * 构造request对象
         */
        function getRequestObject(cmd) {
            var request = new Object();
            request.requestID = getUUID(8, 16);
            request.version = "1.0";
            request.cmd = cmd;
            return request;
        }
        /***
         * 获取自定义区数据以及模板URL
         * waybillNO 电子面单号，此处暂时无法修改
         */
        function getCustomAreaData(waybillNO) {
            //获取waybill对应的自定义区的JSON object，此处的ajaxGet函数是伪代码,此处需修改
            var jsonObject = ajaxGet(waybillNO);
            var ret = new Object();
            ret.templateURL = jsonObject.content.templateURL;
            ret.data = jsonObject.content.data;
            return ret;
        }
        /***
         * 获取电子面单Json 数据
         * waybillNO 电子面单号
         */
        function getWaybillJson(waybillNO) {
            //用TMS_WAYBILL_GET获取云打印接口即可，模板url自定义选择
            //获取waybill对应的json object，此处的ajaxGet函数是伪代码，此处需修改
            var jsonObject = ajaxGet(waybillNO);
            $.get('http://localhost:54439/api/MianDan', function (data) {
                
            })
            return jsonObject;
        }
        /**
         * 请求打印机列表demo
         * */
        function doGetPrinters() {
            var request = getRequestObject("getPrinters");
            webSocket.send(JSON.stringify(request));
        }

        /**
         * 弹窗模式配置打印机
         * */
        function doPrinterConfig() {
            var request = getRequestObject("printerConfig");
            webSocket.send(JSON.stringify(request));
        }
        /***
        * 打印机配置
        */
        function doSetPrinterConfig() {
            var request = getRequestObject("setPrinterConfig");
            request.printer = new Object();
            request.printer.name = defaultPrinter;
            request.printer.needUpLogo = true;
            request.printer.needDownLogo = false;
            webSocket.send(JSON.stringify(request));
        }

        /**
         * 打印电子面单
         * printer 指定要使用那台打印机
         * waybillArray 要打印的电子面单的数组
         */
        function doPrint(printer, waybillArray) {
            var request = getRequestObject("print");
            request.task = new Object();
            request.task.taskID = getUUID(8, 10);
            request.task.preview = false;
            request.task.printer = printer;
            var documents = new Array();
            for (i = 0; i < waybillArray.length; i++) {
                var doc = new Object();
                doc.documentID = waybillArray[i];
                var content = new Array();
                var waybillJson = getWaybillJson(waybillArray[i]);
                var customAreaData = getCustomAreaData(waybillArray[i]);
                content.push(waybillJson, customAreaData);
                doc.content = content;
                documents.push(doc);
            }
            request.task.documents = documents;
            socket.send(JSON.stringify(request));
        }
        ///**
        // * 响应请求demo
        // * */
        //websocket.onmessage = function (event) {
        //    var response = eval(event.data);
        //    if (response.cmd == 'getPrinters') {
        //        getPrintersHandler(response);//处理打印机列表
        //    } else if (response.cmd == 'printerConfig') {
        //        printConfigHandler(response);
        //    }
        //};
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
