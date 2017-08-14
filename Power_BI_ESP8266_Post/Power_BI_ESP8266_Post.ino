/**
   Authorization.ino

    Created on: 09.12.2015

*/

#include <Arduino.h>

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

#include <ESP8266HTTPClient.h>

#define USE_SERIAL Serial

ESP8266WiFiMulti WiFiMulti;
const char* ssid = "HonestAbesWifi";
const char* password = "Sax0ph0ne";

void setup() {

  USE_SERIAL.begin(115200);
  // USE_SERIAL.setDebugOutput(true);

  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();
  /*
      for(uint8_t t = 4; t > 0; t--) {
          USE_SERIAL.printf("[SETUP] WAIT %d...\n", t);
          USE_SERIAL.flush();
          delay(1000);
      }

      WiFiMulti.addAP(ssid, password);
  */
  Serial.write("Setup has been called!");
  WiFi.begin(ssid, password);
  Serial.println("");

  // Wait for connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("WE ARE CONNNNECTED!");
  Serial.print("Connected to ");
  Serial.println(ssid);
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

}
//
// "[\r\n{\r\n\"Sensor\" :\"AAAAA555555\",\r\n\"DateTime\" :\"2017-08-14T22:25:43.970Z\",\r\n\"Reading\" :98.6\r\n}\r\n]"
//"https://api.powerbi.com/beta/72862590-c7d7-4a14-9db8-a58e052d0001/datasets/b631142f-602e-45a7-b160-f579a6d4b850/rows?key=q7bOLIcHBLtKW0GQ%2Bxxt2iieDgNxhUpw%2Fi4bEeuYJw2wW1MGw0NyEvL4bubcLLC1qNiyduJdaRexUkRRQLhX%2Bg%3D%3D"
void loop()
{
  //successfulHTTPPost();
  //successfulHTTPGet();
}

void successfulHTTPPost()
{
  if((WiFiMulti.run() == WL_CONNECTED)) {

        HTTPClient http;

        USE_SERIAL.print("[HTTP] begin...\n");
        // configure traged server and url
        //http.begin("https://192.168.1.12/test.html", "7a 9c f4 db 40 d3 62 5a 6e 21 bc 5c cc 66 c8 3e a1 45 59 38"); //HTTPS
        http.begin("http://jsonplaceholder.typicode.com/posts/"); //HTTP

        USE_SERIAL.print("[HTTP] POST...\n");
        // start connection and send HTTP header
        int httpCode = http.POST("[\r\n{\r\n\"title\" :\"foo\",\r\n\"body\" :\"bar\",\r\n\"userId\" : 1\r\n}\r\n]");

        // httpCode will be negative on error
        if(httpCode > 0) {
            // HTTP header has been send and Server response header has been handled
            USE_SERIAL.printf("[HTTP] POST... code: %d\n", httpCode);

            // file found at server
            if(httpCode == HTTP_CODE_OK || httpCode == 201) {
                String payload = http.getString();
                USE_SERIAL.println(payload);
                Serial.println("Status Code OK!");
            }
            else
            {
              Serial.println("status is not okay :");
              Serial.println(httpCode + http.errorToString(httpCode).c_str());
            }
        } else {
            USE_SERIAL.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
        }

        http.end();
    }
    delay(10000);
  
}

void successfulHTTPGet()
{
if((WiFiMulti.run() == WL_CONNECTED)) {

        HTTPClient http;

        USE_SERIAL.print("[HTTP] begin...\n");
        // configure traged server and url
        //http.begin("https://192.168.1.12/test.html", "7a 9c f4 db 40 d3 62 5a 6e 21 bc 5c cc 66 c8 3e a1 45 59 38"); //HTTPS
        http.begin("http://jsonplaceholder.typicode.com/users/1"); //HTTP

        USE_SERIAL.print("[HTTP] GET...\n");
        // start connection and send HTTP header
        int httpCode = http.GET();

        // httpCode will be negative on error
        if(httpCode > 0) {
            // HTTP header has been send and Server response header has been handled
            USE_SERIAL.printf("[HTTP] GET... code: %d\n", httpCode);

            // file found at server
            if(httpCode == HTTP_CODE_OK) {
                String payload = http.getString();
                USE_SERIAL.println(payload);
            }
        } else {
            USE_SERIAL.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
        }

        http.end();
    }

    delay(10000);
}
  /*
    http.begin("https://api.powerbi.com/beta/72862590-c7d7-4a14-9db8-a58e052d0001/datasets/b631142f-602e-45a7-b160-f579a6d4b850/rows?key=q7bOLIcHBLtKW0GQ%2Bxxt2iieDgNxhUpw%2Fi4bEeuYJw2wW1MGw0NyEvL4bubcLLC1qNiyduJdaRexUkRRQLhX%2Bg%3D%3D");
    http.addHeader("Content-Type", "application/json");
    int httpCode = http.POST("[\r\n{\r\n\"Sensor\" :\"Cube\",\r\n\"DateTime\" :\"2017-08-04T13:31:56.643Z\",\r\n\"Reading\" :10\r\n}\r\n]");

   */
 
