#include <Servo.h>
Servo myservo;  // create servo object to control a servo

char idxChar[4];
String recByte;
int idxC;
char dummy = 0;char bRec[160];char *n;
int idx,trayFl=0,ctr=0;
int state1,state2,state3,state4,state5,state6,state7,state8;
int st1,st2,st3,st4,st5,st6,st7,st8;

#define led1 22
#define led2 24
#define led3 26
#define led4 28
#define led5 30
#define led6 32
#define led7 34
#define led8 36

const int sw1 = 23;
const int sw2 = 25;
const int sw3 = 27;
const int sw4 = 29;
const int sw5 = 31;
const int sw6 = 33;
const int sw7 = 35;
const int sw8 = 37;

const int traySw = 9;
char rxByte;

void setup() {
  // put your setup code here, to run once:
  delay(100);
  Serial.begin(9600);Serial1.begin(9600); 
  Serial.print("READY");
  myservo.attach(8);  // attaches the servo on pin 9 to the servo object
  pinMode(led1,OUTPUT);pinMode(led2,OUTPUT);
  pinMode(led3,OUTPUT);pinMode(led4,OUTPUT);
  pinMode(led5,OUTPUT);pinMode(led6,OUTPUT);
  pinMode(led7,OUTPUT);pinMode(led8,OUTPUT);

  pinMode(sw1,INPUT_PULLUP);pinMode(sw2,INPUT_PULLUP);
  pinMode(sw3,INPUT_PULLUP);pinMode(sw4,INPUT_PULLUP);
  pinMode(sw5,INPUT_PULLUP);pinMode(sw6,INPUT_PULLUP);
  pinMode(sw7,INPUT_PULLUP);pinMode(sw8,INPUT_PULLUP);  
  pinMode(traySw,INPUT_PULLUP);
}

void loop() {

  if(Serial.available())
    {
      rxByte = Serial.read();
      if(rxByte == '1')
        {
          if(digitalRead(led1)==1)
            {
              digitalWrite(led1,0);Serial.println("LED1_OFF");
            }
          else
            {
              digitalWrite(led1,1);Serial.println("LED1_ON"); openTray(); 
            }
        }
      else if(rxByte == '2') 
        {
          if(digitalRead(led2)==1)
            {
              digitalWrite(led2,0);Serial.println("LED2_OFF");
            }
          else
            {
              digitalWrite(led2,1);Serial.println("LED2_ON"); openTray();  
            }
        }
      else if(rxByte == '3') 
        {
          if(digitalRead(led3)==1)
            {
              digitalWrite(led3,0);Serial.println("LED3_OFF");
            }
          else
            {
              digitalWrite(led3,1);Serial.println("LED3_ON"); openTray();
            }
        }
      else if(rxByte == '4') 
        {
          if(digitalRead(led4)==1)
            {
              digitalWrite(led4,0);Serial.println("LED4_OFF");
            }
          else
            {
              digitalWrite(led4,1);Serial.println("LED4_ON"); openTray(); 
            }
        }
      else if(rxByte == '5') 
        {
          if(digitalRead(led5)==1)
            {
              digitalWrite(led5,0);Serial.println("LED5_OFF");
            }
          else
            {
              digitalWrite(led5,1);Serial.println("LED5_ON"); openTray(); 
            }
        }
      else if(rxByte == '6') 
        {
          if(digitalRead(led6)==1)
            {
              digitalWrite(led6,0);Serial.println("LED6_OFF");
            }
          else
            {
              digitalWrite(led6,1);Serial.println("LED6_ON"); openTray(); 
            }
        }
      else if(rxByte == '7') 
        {
          if(digitalRead(led7)==1)
            {
              digitalWrite(led7,0);Serial.println("LED7_OFF");
            }
          else
            {
              digitalWrite(led7,1);Serial.println("LED7_ON"); openTray(); 
            }
        }
      else if(rxByte == '8') 
        {
          if(digitalRead(led8)==1)
            {
              digitalWrite(led8,0);Serial.println("LED8_OFF");
            }
          else
            {
              digitalWrite(led8,1);Serial.println("LED8_ON"); openTray(); 
            }
        }
      else if(rxByte == 'A')//ask for status for all switches 
        {
          Serial.print("DATA_");
          Serial.print(digitalRead(sw1));Serial.print(digitalRead(sw2));
          Serial.print(digitalRead(sw3));Serial.print(digitalRead(sw4));
          Serial.print(digitalRead(sw5));Serial.print(digitalRead(sw6));
          Serial.print(digitalRead(sw7));Serial.print(digitalRead(sw8));
          Serial.println("_OK");
        }
      else if(rxByte == 'B')//turn off all leds 
        {
          
        }
      else if(rxByte == 'C') 
        {
          
        }
      else if(rxByte == 'D') 
        {
          
        }
    }
}
void sendState()
{
  Serial.print("DATA_");
  Serial.print(digitalRead(sw1));Serial.print(digitalRead(sw2));
  Serial.print(digitalRead(sw3));Serial.print(digitalRead(sw4));
  Serial.print(digitalRead(sw5));Serial.print(digitalRead(sw6));
  Serial.print(digitalRead(sw7));Serial.print(digitalRead(sw8));
  Serial.println("_OK");
}
void openTray()
{
  Serial.println("DATA_TRAY_OPEN");
  trayFl=1;
  state1 = digitalRead(sw1); state2 = digitalRead(sw2); 
  state3 = digitalRead(sw3); state4 = digitalRead(sw4); 
  state5 = digitalRead(sw5); state6 = digitalRead(sw6); 
  state7 = digitalRead(sw7); state8 = digitalRead(sw8);  
//  for(int x = 1;x<=30;x++)
//    {
//      myservo.write(45); // open tray
//      delay(20); 
//    }

  delay(500);  
  sendState();
  ctr=0;
  
  while(1)
  {
    st1 = digitalRead(sw1); st2 = digitalRead(sw2); 
    st3 = digitalRead(sw3); st4 = digitalRead(sw4); 
    st5 = digitalRead(sw5); st6 = digitalRead(sw6); 
    st7 = digitalRead(sw7); st8 = digitalRead(sw8); 
//    delay(1000);
    if(st1 != state1)
      {
        if(digitalRead(sw1)==1)
          {
            Serial.println("DATA_SW1_OFF");             
          }
        else
          {
            Serial.println("DATA_SW1_ON");  
          }
        delay(500);
        sendState();          
        state1 = st1; 
      }
    if(st2 != state2)
      {
        if(digitalRead(sw2)==1)
          Serial.println("DATA_SW2_OFF");             
        else
          Serial.println("DATA_SW2_ON");  
        delay(500);  
        sendState();
        state2 = st2; 
      }
    if(st3 != state3)
      {
        if(digitalRead(sw3)==1)
          Serial.println("DATA_SW3_OFF");             
        else
          Serial.println("DATA_SW3_ON");  
        delay(500);  
        sendState();  
        state3 = st3;
      }
    if(st4 != state4)
      {
        if(digitalRead(sw4)==1)
          Serial.println("DATA_SW4_OFF");             
        else
          Serial.println("DATA_SW4_ON");  
        delay(500);  
        sendState();  
        state4 = st4;
      }
    if(st5 != state5)
      {
        if(digitalRead(sw5)==1)
          Serial.println("DATA_SW5_OFF");             
        else
          Serial.println("DATA_SW5_ON");  
        delay(500);  
        sendState();  
        state5 = st5;
      }
    if(st6 != state6)
      {
        if(digitalRead(sw6)==1)
          Serial.println("DATA_SW6_OFF");             
        else
          Serial.println("DATA_SW6_ON");  
        delay(500);  
        sendState();  
        state6 = st6;              
      }
    if(st7 != state7)
      {
        if(digitalRead(sw7)==1)
          Serial.println("DATA_SW7_OFF");             
        else
          Serial.println("DATA_SW7_ON");  
        delay(500);  
        sendState();  
        state7 = st7; 
      }
    if(st8 != state8)
      {
        if(digitalRead(sw8)==1)
          Serial.println("DATA_SW8_OFF");             
        else
          Serial.println("DATA_SW8_ON");  
        delay(500);  
        sendState();  
        state8 = st8;  
      }
    delay(100);ctr++;
    if(ctr >= 600)  
      {
        Serial.print("DATA_");
        Serial.print(digitalRead(sw1));Serial.print(digitalRead(sw2));
        Serial.print(digitalRead(sw3));Serial.print(digitalRead(sw4));
        Serial.print(digitalRead(sw5));Serial.print(digitalRead(sw6));
        Serial.print(digitalRead(sw7));Serial.print(digitalRead(sw8));
        Serial.println("_OK");ctr = 0; 
      }
    if(digitalRead(traySw)==0)  
      {
        Serial.println("DATA_TRAY_CLOSED");trayFl = 0;
        digitalWrite(led1,0);digitalWrite(led2,0);
        digitalWrite(led3,0);digitalWrite(led4,0);
        digitalWrite(led5,0);digitalWrite(led6,0);
        digitalWrite(led7,0);digitalWrite(led8,0);
        break;
      }
  }
}
void closeTray()// close tray
{
  trayFl=0;
  for(int x = 1;x<=30;x++)
    {
      myservo.write(135); // tell servo to go to position
      delay(20); 
    }  
}
void testCode()
{
// Serial.print("U");
  getIdx();
  Serial.println(idxC);
  if(idxC == 1)
    {      
      digitalWrite(led1,1); while(digitalRead(sw1)==HIGH); digitalWrite(led1,0);
    }
  else if(idxC == 2)
    {
      digitalWrite(led2,1); while(digitalRead(sw2)==1); digitalWrite(led2,0);
    }
  else if(idxC == 3)
    {
      digitalWrite(led3,1); while(digitalRead(sw3)==1); digitalWrite(led3,0);
    }
  else if(idxC == 4)
    {
      digitalWrite(led4,1); while(digitalRead(sw4)==1); digitalWrite(led4,0);
    }
  else if(idxC == 5)
    {
      digitalWrite(led5,1); while(digitalRead(sw5)==1); digitalWrite(led5,0);
    }
  else if(idxC == 6)
    {
      digitalWrite(led6,1); while(digitalRead(sw6)==1); digitalWrite(led6,0);
    }
  else if(idxC == 7)
    {
      digitalWrite(led7,1); while(digitalRead(sw7)==1); digitalWrite(led7,0);
    }
  else if(idxC == 8)
    {
      digitalWrite(led8,1); while(digitalRead(sw8)==1); digitalWrite(led8,0);
    }  
}
/**********************************************************************************/
void getIdx()
{
    Serial.flush();
    dummy = 0;
    
    while(!Serial.available());  
  
    idx = 0;
  
    do
    {
      if(Serial.available() > 0)
        {
          dummy = Serial.read();
          bRec [idx] = dummy;
          idx++;          
        }        
    }while(dummy != 'K'); 
    bRec[idx] = '\0';
    Serial.print("bREC ");Serial.println(bRec);
    n = strtok(bRec,"_");
    n = strtok(NULL,"_");strcpy(idxChar,n);     

    recByte = String(idxChar);
    idxC = atoi(idxChar);       
//    Serial.print("INDEX: ");Serial.println(idxC);
}

