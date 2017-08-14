/**
 * Authorization.ino
 *
 *  Created on: 09.12.2015
 *
 */

#include <Arduino.h>

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

#include <ESP8266HTTPClient.h>

#define USE_SERIAL Serial

ESP8266WiFiMulti WiFiMulti;
const char* ssid = "PlusMobile";
const char* password = "Consult1370!!";

void setup() {

    USE_SERIAL.begin(115200);
   // USE_SERIAL.setDebugOutput(true);

    USE_SERIAL.println();
    USE_SERIAL.println();
    USE_SERIAL.println();

    for(uint8_t t = 4; t > 0; t--) {
        USE_SERIAL.printf("[SETUP] WAIT %d...\n", t);
        USE_SERIAL.flush();
        delay(1000);
    }

    WiFiMulti.addAP(ssid, password);

}

void loop() {
    delay(1000); 
    // wait for WiFi connection
    if((WiFiMulti.run() == WL_CONNECTED)) 
    {

      HTTPClient http;
      
      USE_SERIAL.print("[HTTP] begin...\n");
      // configure traged server and url

      http.begin("https://api.powerbi.com/beta/72862590-c7d7-4a14-9db8-a58e052d0001/datasets/b631142f-602e-45a7-b160-f579a6d4b850/rows?key=q7bOLIcHBLtKW0GQ%2Bxxt2iieDgNxhUpw%2Fi4bEeuYJw2wW1MGw0NyEvL4bubcLLC1qNiyduJdaRexUkRRQLhX%2Bg%3D%3D");
      http.addHeader("Content-Type", "application/json");
      int httpCode = http.POST("[\r\n{\r\n\"Sensor\" :\"Cube\",\r\n\"DateTime\" :\"2017-08-04T13:31:56.643Z\",\r\n\"Reading\" :10\r\n}\r\n]");  
      String payload = http.getString();
      Serial.println(httpCode);   
      Serial.println(payload);
      http.end();
    }

    delay(10000);
}

