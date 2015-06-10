#include "ucp.h"

void setButtonPacketField(buttonPacket_t* buttonPacket)
{
	buttonPacket->start_flag=NEW_PACKET;
	buttonPacket->command=BUTTON_COMMAND_TYPE;
	if(GPIO_ReadInputDataBit(GPIOA, GPIO_Pin_0))
	{
		buttonPacket->butt1_state=BUTTON_CLICKED;
		GPIO_SetBits(GPIOD, GPIO_Pin_14);
	}
	else
	{
		buttonPacket->butt1_state=BUTTON_NO_CLICKED;
		GPIO_ResetBits(GPIOD,GPIO_Pin_14);
	}
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

void setLedSequence(ledSequencePacket_t* ledSequencePacket, int ledCounter)
{
	if(ledSeqPacket->crc==CRC_START)
	{
		if (ledSequencePacket->sequence_number==LED_ACCORDING_TO_CLOCK)
		{
			if(ledCounter==0)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_12);
				return;
			}
			if(ledCounter==15)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_13);
				return;
			}
			if(ledCounter==30)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_14);
				return;
			}
			if(ledCounter==45)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_15);
				return;
			}
		}
		if (ledSequencePacket->sequence_number==LED_REVERSE_AS_CLOCK)
		{
			if(ledCounter==0)
			{
				GPIO_SetBits(GPIOD, GPIO_Pin_15);
				return;
			}
			if(ledCounter==15)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_14);
				return;
			}
			if(ledCounter==30)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_13);
				return;
			}
			if(ledCounter==60)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				GPIO_SetBits(GPIOD, GPIO_Pin_12);
				return;
			}
		}
		if (ledSequencePacket->sequence_number==LED_ALL_LEDS)
			{
				GPIO_SetBits(GPIOD, GPIO_Pin_12 | GPIO_Pin_13 | GPIO_Pin_14 | GPIO_Pin_15);
				return;
			}
		if (ledSequencePacket->sequence_number==LED_NO_LEDS)
			{
				GPIO_ResetBits(GPIOD,GPIO_Pin_All);
				return;
			}
	}
}
