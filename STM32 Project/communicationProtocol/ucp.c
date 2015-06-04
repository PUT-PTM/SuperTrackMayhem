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

void setAccelerometerPacketField(accPacket_t* accPacket, int8_t przyspieszenie_x, int8_t przyspieszenie_y, int8_t przyspieszenie_z)
{
	accPacket->start_flag=NEW_PACKET;
	accPacket->command=ACC_COMMAND_TYPE;
	accPacket->x=przyspieszenie_x;
	accPacket->y=(przyspieszenie_y+4)/128*9.8;
	accPacket->z=przyspieszenie_z;
	accPacket->crc=CRC_START;

}

void initButton(GPIO_InitTypeDef* GPIO_InitStructure)
{
	GPIO_InitStructure->GPIO_Pin = GPIO_Pin_0;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_IN;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
}

void initLeds(GPIO_InitTypeDef* GPIO_InitStructure)
{
	GPIO_InitStructure->GPIO_Pin = GPIO_Pin_12 | GPIO_Pin_13| GPIO_Pin_14| GPIO_Pin_15;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure->GPIO_PuPd = GPIO_PuPd_NOPULL;
	GPIO_Init(GPIOD, &GPIO_InitStructure);
}
void initTim2(TIM_TimeBaseInitTypeDef* TIM_TimeBaseStructure)
{
	 TIM_TimeBaseStructure->TIM_Period = 1999;
	 TIM_TimeBaseStructure->TIM_Prescaler = 550 - 1;
	 TIM_TimeBaseStructure->TIM_ClockDivision = TIM_CKD_DIV1;
	 TIM_TimeBaseStructure->TIM_CounterMode = TIM_CounterMode_Up ;
	 TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);
}

void asixNormalization(int8_t* przyspieszenie_x, int8_t* przyspieszenie_y, int8_t* przyspieszenie_z)
{
	if(*przyspieszenie_x>127)
	{
		*przyspieszenie_x=*przyspieszenie_x-1;
		*przyspieszenie_x=(~*przyspieszenie_x)&0xFF;
		*przyspieszenie_x=-*przyspieszenie_x;
	}
	if(*przyspieszenie_y>127)
	{
		 *przyspieszenie_y=*przyspieszenie_y-1;
		 *przyspieszenie_y=(~*przyspieszenie_y)&0xFF;
		 *przyspieszenie_y=-*przyspieszenie_y;
	}
	if(*przyspieszenie_z>127)
	{
		*przyspieszenie_z=*przyspieszenie_z-1;
		*przyspieszenie_z=(~*przyspieszenie_z)&0xFF;
		*przyspieszenie_z=-*przyspieszenie_z;
	}
}

void initLIS302DL(LIS302DL_InitTypeDef*  LIS302DL_InitStruct, LIS302DL_InterruptConfigTypeDef* LIS302DL_InterruptStruct)
{
	/* Set configuration of LIS302DL*/
	LIS302DL_InitStruct->Power_Mode = LIS302DL_LOWPOWERMODE_ACTIVE;
	LIS302DL_InitStruct->Output_DataRate = LIS302DL_DATARATE_100;
	LIS302DL_InitStruct->Axes_Enable = LIS302DL_X_ENABLE | LIS302DL_Y_ENABLE | LIS302DL_Z_ENABLE;
	LIS302DL_InitStruct->Full_Scale = LIS302DL_FULLSCALE_2_3;
	LIS302DL_InitStruct->Self_Test = LIS302DL_SELFTEST_NORMAL;
	LIS302DL_Init(&LIS302DL_InitStruct);

	/* Set configuration of Internal High Pass Filter of LIS302DL*/
	LIS302DL_InterruptStruct->Latch_Request = LIS302DL_INTERRUPTREQUEST_LATCHED;
	LIS302DL_InterruptStruct->SingleClick_Axes = LIS302DL_CLICKINTERRUPT_Z_ENABLE;
	LIS302DL_InterruptStruct->DoubleClick_Axes = LIS302DL_DOUBLECLICKINTERRUPT_Z_ENABLE;
	LIS302DL_InterruptConfig(&LIS302DL_InterruptStruct);
}

void initSPI(GPIO_InitTypeDef* GPIO_InitStructure, SPI_InitTypeDef*  SPI_InitStructure)
{
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_AF;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_PuPd  = GPIO_PuPd_DOWN;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_50MHz;

	/* SPI SCK pin configuration */
	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_SCK_PIN;
	GPIO_Init(LIS302DL_SPI_SCK_GPIO_PORT, &GPIO_InitStructure);

	/* SPI  MOSI pin configuration */
	GPIO_InitStructure->GPIO_Pin =  LIS302DL_SPI_MOSI_PIN;
	GPIO_Init(LIS302DL_SPI_MOSI_GPIO_PORT, &GPIO_InitStructure);

	/* SPI MISO pin configuration */
	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_MISO_PIN;
	GPIO_Init(LIS302DL_SPI_MISO_GPIO_PORT, &GPIO_InitStructure);

	/* SPI configuration -------------------------------------------------------*/
	SPI_I2S_DeInit(LIS302DL_SPI);
	SPI_InitStructure->SPI_Direction = SPI_Direction_2Lines_FullDuplex;
	SPI_InitStructure->SPI_DataSize = SPI_DataSize_8b;
	SPI_InitStructure->SPI_CPOL = SPI_CPOL_Low;
	SPI_InitStructure->SPI_CPHA = SPI_CPHA_1Edge;
	SPI_InitStructure->SPI_NSS = SPI_NSS_Soft;
	SPI_InitStructure->SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_4;
	SPI_InitStructure->SPI_FirstBit = SPI_FirstBit_MSB;
	SPI_InitStructure->SPI_CRCPolynomial = 7;
	SPI_InitStructure->SPI_Mode = SPI_Mode_Master;
	SPI_Init(LIS302DL_SPI, &SPI_InitStructure);
}

void configGPIOforListChip(GPIO_InitTypeDef* GPIO_InitStructure)
{
	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_CS_PIN;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(LIS302DL_SPI_CS_GPIO_PORT, &GPIO_InitStructure);

	/* Deselect : Chip Select high */
	GPIO_SetBits(LIS302DL_SPI_CS_GPIO_PORT, LIS302DL_SPI_CS_PIN);

	/* Configure GPIO PINs to detect Interrupts */
	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_INT1_PIN;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_IN;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure->GPIO_PuPd  = GPIO_PuPd_NOPULL;
	GPIO_Init(LIS302DL_SPI_INT1_GPIO_PORT, &GPIO_InitStructure);

	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_INT2_PIN;
	GPIO_Init(LIS302DL_SPI_INT2_GPIO_PORT, &GPIO_InitStructure);
}

