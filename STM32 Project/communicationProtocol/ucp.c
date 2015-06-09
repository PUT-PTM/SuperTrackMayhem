#include "ucp.h"

void setButtonPacketField(buttonPacket_t* buttonPacket)
{
	buttonPacket->start_flag=NEW_PACKET;
	buttonPacket->command=BUTTON_COMMAND_TYPE;
	if(GPIO_ReadInputDataBit(GPIOA, GPIO_Pin_0))
		buttonPacket->butt1_state=BUTTON_CLICKED;
	else
		buttonPacket->butt1_state=BUTTON_NO_CLICKED;
	buttonPacket->butt2_state=BUTTON_NO_CLICKED;
	buttonPacket->butt3_state=BUTTON_NO_CLICKED;
	buttonPacket->butt4_state=BUTTON_NO_CLICKED;
}

void setAccelerometerPacketField(accPacket_t* accPacket, int8_t acceleration_x, int8_t acceleration_y, int8_t acceleration_z)
{
	accPacket->x=acceleration_x;
	accPacket->y=(acceleration_y*9.8f)/128;
	accPacket->z=acceleration_z;
	accPacket->crc=CRC_START;
}

void asixNormalization(int8_t* acceleration_x, int8_t* acceleration_y, int8_t* acceleration_z)
{
	if(*acceleration_x>127)
	{
		*acceleration_x=*acceleration_x-1;
		*acceleration_x=(~*acceleration_x)&0xFF;
		*acceleration_x=-*acceleration_x;
	}
	if(*acceleration_y>127)
	{
		 *acceleration_y=*acceleration_y-1;
		 *acceleration_y=(~*acceleration_y)&0xFF;
		 *acceleration_y=-*acceleration_y;
	}
	if(*acceleration_z>127)
	{
		*acceleration_z=*acceleration_z-1;
		*acceleration_z=(~*acceleration_z)&0xFF;
		*acceleration_z=-*acceleration_z;
	}
}
