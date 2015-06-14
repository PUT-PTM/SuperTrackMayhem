#include "stm32f4xx_tim.h"
#include "accelerometer.h"

#define SYNC_COMAND 0x01
#define PROTOCOL_V1 0x01
#define NEW_PACKET 0xAA

#define ACC_COMMAND_TYPE 0xAC
#define ACC_PACKET_SIZE 80

#define CRC_START 99

#define BUTTON_COMMAND_TYPE 0x38
#define BUTTON_CLICKED 0xFF
#define BUTTON_NO_CLICKED 0x00

#define LED_SEQUENCE 0xEE
#define LED_NO_LEDS 0x00
#define LED_ACCORDING_TO_CLOCK 0x01
#define LED_REVERSE_AS_CLOCK 0x02
#define LED_ALL_LEDS 0x03

#define LED_PACKET 0xED
#define LED_ON 0xFF
#define LED_OFF 0x00

typedef struct __attribute__((packed)) syncPacket
{
	uint8_t start_flag;
	uint8_t command;
	uint8_t protocol_version;
}syncPacket_t;

typedef struct __attribute__((packed)) accPacket
{
	uint8_t start_flag;
	uint8_t command;
	float x;
	float y;
	float z;
	uint8_t crc;
}accPacket_t;

typedef struct __attribute__((packed)) buttonPacket
{
	uint8_t start_flag;
	uint8_t command;
	uint8_t butt1_state;
	uint8_t butt2_state;
	uint8_t butt3_state;
	uint8_t butt4_state;
	uint8_t crc;
}buttonPacket_t;

typedef struct __attribute__((packed)) ledPacket
{
	uint8_t start_flag;
	uint8_t command;
	uint8_t led1_state;
	uint8_t led2_state;
	uint8_t led3_state;
	uint8_t led4_state;
	uint8_t crc;
}ledPacket_t;


typedef struct __attribute__((packed)) ledSequencePacket
{
	uint8_t start_flag;
	uint8_t command;
	uint8_t sequence_number;
	uint8_t crc;
}ledSequencePacket_t;

void asixNormalization(int8_t* acceleration_x, int8_t* acceleration_y, int8_t* acceleration_z);
void setAccelerometerPacketField(accPacket_t* accPacket, int8_t acceleration_x, int8_t acceleration_y, int8_t acceleration_z);
void setButtonPacketField(buttonPacket_t* buttonPacket );
void setLedSequence(ledSequencePacket_t* ledSequencePacket, int ledCounter);
void setLedState(ledPacket_t* ledPacket);
