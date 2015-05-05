#include <stdlib.h>
#include <stdio.h>
#include "misc.h"

#define ACC_COMMAND_TYPE 0xAA
#define ACC_MAX_SCOPE 511
#define CRC_START 99
#define ACC_PACKET_SIZE 80

#define BUTTON_COMMAND_TYPE 0x38
#define BUTTON_CLICKED 0xFF
#define BUTTON_NO_CLICKED 0x00

typedef struct __attribute__((packed)) accPacket
{
	uint8_t command;
	int16_t x;
	int16_t y;
	int16_t z;
	int16_t max;
	uint8_t crc;
}accPacket_t;

typedef struct __attribute__((packed)) buttonPacket
{
	uint8_t command;
	uint8_t butt1_state;
	uint8_t butt2_state;
	uint8_t butt3_state;
	uint8_t butt4_state;
	uint8_t crc;
}buttonPacket_t;
