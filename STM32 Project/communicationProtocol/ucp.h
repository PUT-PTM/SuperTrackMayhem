#include <stdlib.h>
#include <stdio.h>
#include "misc.h"

#define SYNC_COMAND 0x01
#define PROTOCOL_V1 0x01
#define NEW_PACKET 0xAA

#define ACC_COMMAND_TYPE 0xAC
#define ACC_PACKET_SIZE 80

#define CRC_START 99

#define BUTTON_COMMAND_TYPE 0x38
#define BUTTON_CLICKED 0xFF
#define BUTTON_NO_CLICKED 0x00

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

typedef struct __attribute__((packed)) ledSequencePacket
{
	uint8_t start_flag;
	uint8_t command;
	uint8_t sequence_number;
}ledSequencePacket_t;
