#include "main.h"

int main(void)
{
    SystemInit();
    init();

    /* initialize the CPU time distribution */
    SystemCoreClockUpdate(); //

    /* enable clocks for GPIOA, SPI, SCK, MOSI, MISO, CS GPIO, INT1 GPIO, INT2 GPIO, TIM2, GPIOD */
    enableClocks();

    gpio_PinAFConfig();

    GPIO_InitTypeDef GPIO_InitStructure;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF;
    GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
    GPIO_InitStructure.GPIO_PuPd  = GPIO_PuPd_DOWN;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;

    /* SPI SCK pin configuration */
    GPIO_InitStructure.GPIO_Pin = LIS302DL_SPI_SCK_PIN;
    GPIO_Init(LIS302DL_SPI_SCK_GPIO_PORT, &GPIO_InitStructure);

    /* SPI  MOSI pin configuration */
    GPIO_InitStructure.GPIO_Pin =  LIS302DL_SPI_MOSI_PIN;
    GPIO_Init(LIS302DL_SPI_MOSI_GPIO_PORT, &GPIO_InitStructure);

    /* SPI MISO pin configuration */
    GPIO_InitStructure.GPIO_Pin = LIS302DL_SPI_MISO_PIN;
    GPIO_Init(LIS302DL_SPI_MISO_GPIO_PORT, &GPIO_InitStructure);

    /* SPI configuration -------------------------------------------------------*/
    SPI_InitTypeDef  SPI_InitStructure;
    SPI_I2S_DeInit(LIS302DL_SPI);
    SPI_InitStructure.SPI_Direction = SPI_Direction_2Lines_FullDuplex;
    SPI_InitStructure.SPI_DataSize = SPI_DataSize_8b;
    SPI_InitStructure.SPI_CPOL = SPI_CPOL_Low;
    SPI_InitStructure.SPI_CPHA = SPI_CPHA_1Edge;
    SPI_InitStructure.SPI_NSS = SPI_NSS_Soft;
    SPI_InitStructure.SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_4;
    SPI_InitStructure.SPI_FirstBit = SPI_FirstBit_MSB;
    SPI_InitStructure.SPI_CRCPolynomial = 7;
    SPI_InitStructure.SPI_Mode = SPI_Mode_Master;

    SPI_Init(LIS302DL_SPI, &SPI_InitStructure);

    /* Enable SPI1  */
    SPI_Cmd(LIS302DL_SPI, ENABLE);

    /* Configure GPIO PIN for Lis Chip select */
    GPIO_InitStructure.GPIO_Pin = LIS302DL_SPI_CS_PIN;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_OUT;
    GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_Init(LIS302DL_SPI_CS_GPIO_PORT, &GPIO_InitStructure);

    /* Deselect : Chip Select high */
    GPIO_SetBits(LIS302DL_SPI_CS_GPIO_PORT, LIS302DL_SPI_CS_PIN);

    /* Configure GPIO PINs to detect Interrupts */
    GPIO_InitStructure.GPIO_Pin = LIS302DL_SPI_INT1_PIN;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN;
    GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_PuPd  = GPIO_PuPd_NOPULL;
    GPIO_Init(LIS302DL_SPI_INT1_GPIO_PORT, &GPIO_InitStructure);

    GPIO_InitStructure.GPIO_Pin = LIS302DL_SPI_INT2_PIN;
    GPIO_Init(LIS302DL_SPI_INT2_GPIO_PORT, &GPIO_InitStructure);

    LIS302DL_InitTypeDef  LIS302DL_InitStruct;
    LIS302DL_InterruptConfigTypeDef LIS302DL_InterruptStruct;
    initLIS302DL(&LIS302DL_InitStruct, &LIS302DL_InterruptStruct);
    LIS302DL_Init(&LIS302DL_InitStruct);
    LIS302DL_InterruptConfig(&LIS302DL_InterruptStruct);

    initButton(&GPIO_InitStructure);
    GPIO_Init(GPIOA, &GPIO_InitStructure);

    int8_t acceleration_x, acceleration_y, acceleration_z;
    /* accelerometer packet initialization */
    accPacket_t accPacket;
    accPacket.start_flag=NEW_PACKET;
    accPacket.command=ACC_COMMAND_TYPE;

    /* button packet initialization */
    buttonPacket_t buttonPacket;

    /* led packet initialization */
    ledPacket_t ledPacket;

    /* Leds Configuration */
    initLeds(&GPIO_InitStructure);
    GPIO_Init(GPIOD, &GPIO_InitStructure);

    /* Timer2 Configuration */
    TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;
    TIM_TimeBaseStructure.TIM_Period = 1299;//1999,999
    TIM_TimeBaseStructure.TIM_Prescaler = 550 - 1;
    TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
    TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up ;
    TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);
    //GPIO_SetBits(GPIOD, GPIO_Pin_12 | GPIO_Pin_13 | GPIO_Pin_14 | GPIO_Pin_15);

    /* buffer for incoming packet*/
    uint8_t bufferForReading [4];

    /* buffer for led statet packet*/
    uint8_t bufferForLeds [5];

    /* led counter to do led sequence */
    int ledCounter=0;

    /* led sequence packet initialization */
    ledSequencePacket_t ledSeqPacket;
    while(1)
    {
			TIM_Cmd(TIM2, ENABLE);// /*ENABLE TIMER2 */
			LIS302DL_Read(&acceleration_x, LIS302DL_OUT_X_ADDR, 1);
			LIS302DL_Read(&acceleration_y, LIS302DL_OUT_Y_ADDR, 1);
			LIS302DL_Read(&acceleration_z, LIS302DL_OUT_Z_ADDR, 1);

			asixNormalization(&acceleration_x, &acceleration_y, &acceleration_z);

			/*setting the value of the packet for the accelerometer*/
			setAccelerometerPacketField(&accPacket, acceleration_x, acceleration_y, acceleration_z);
			/*sending accelerometer packet*/
			VCP_send_buffer(&accPacket, sizeof(accPacket_t));

			/*setting the value of the packet for the button*/
			setButtonPacketField(&buttonPacket);
			/*sending button packet*/
			VCP_send_buffer(&buttonPacket, sizeof(buttonPacket_t));

			/* reading incoming packet*/
			VCP_get_char(&bufferForReading[0]);

			if(bufferForReading[0]==NEW_PACKET)
			{
				VCP_get_char(&bufferForReading[1]);

				if(bufferForReading[1]==LED_SEQUENCE)
				{
					VCP_get_char(&bufferForReading[2]);
					VCP_get_char(&bufferForReading[3]);
					ledSeqPacket.start_flag=NEW_PACKET;
					ledSeqPacket.command=LED_SEQUENCE;
					ledSeqPacket.sequence_number=bufferForReading[2];
					ledSeqPacket.crc=bufferForReading[3];
					setLedSequence(&ledSeqPacket, ledCounter);
				}

				if(bufferForReading[1]==LED_PACKET)
				{
					VCP_get_char(&bufferForReading[2]);
					VCP_get_char(&bufferForReading[3]);
					ledPacket.start_flag=NEW_PACKET;
					ledPacket.led1_state=VCP_get_char(&bufferForLeds[0]);
					ledPacket.led2_state=VCP_get_char(&bufferForLeds[1]);
					ledPacket.led3_state=VCP_get_char(&bufferForLeds[2]);
					ledPacket.led4_state=VCP_get_char(&bufferForLeds[3]);
					ledPacket.crc=VCP_get_char(&bufferForLeds[4]);
					setLedState(&ledPacket);
				}
			}

			ledCounter++;
			/*reset led counter*/
			if (ledCounter==60)
				ledCounter=0;
			/* end of reading and handling incoming packet */

			while(1)
				if(TIM_GetFlagStatus(TIM2, TIM_FLAG_Update)) {
					TIM_ClearFlag(TIM2, TIM_FLAG_Update);
					break;
				}


    }


}

void enableClocks()
{
	 /* Enable GPIOA clock */
	 RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOA, ENABLE);

	 /* Enable the SPI periph */
	 RCC_APB2PeriphClockCmd(LIS302DL_SPI_CLK, ENABLE);

	 /* Enable SCK, MOSI and MISO GPIO clocks */
	 RCC_AHB1PeriphClockCmd(LIS302DL_SPI_SCK_GPIO_CLK | LIS302DL_SPI_MISO_GPIO_CLK | LIS302DL_SPI_MOSI_GPIO_CLK, ENABLE);

	 /* Enable CS  GPIO clock */
	 RCC_AHB1PeriphClockCmd(LIS302DL_SPI_CS_GPIO_CLK, ENABLE);

	 /* Enable INT1 GPIO clock */
	 RCC_AHB1PeriphClockCmd(LIS302DL_SPI_INT1_GPIO_CLK, ENABLE);

	 /* Enable INT2 GPIO clock */
	 RCC_AHB1PeriphClockCmd(LIS302DL_SPI_INT2_GPIO_CLK, ENABLE);

	 /* Enable TIM2 clock */
	 RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM2, ENABLE);

	 /* Enable GPIOD clock */
	 RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOD, ENABLE);
}

void gpio_PinAFConfig()
{
	 GPIO_PinAFConfig(LIS302DL_SPI_SCK_GPIO_PORT, LIS302DL_SPI_SCK_SOURCE, LIS302DL_SPI_SCK_AF);
	 GPIO_PinAFConfig(LIS302DL_SPI_MISO_GPIO_PORT, LIS302DL_SPI_MISO_SOURCE, LIS302DL_SPI_MISO_AF);
	 GPIO_PinAFConfig(LIS302DL_SPI_MOSI_GPIO_PORT, LIS302DL_SPI_MOSI_SOURCE, LIS302DL_SPI_MOSI_AF);
}

void initButton(GPIO_InitTypeDef* GPIO_InitStructure)
{
	GPIO_InitStructure->GPIO_Pin = GPIO_Pin_0;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_IN;
}

void initLeds(GPIO_InitTypeDef* GPIO_InitStructure)
{
	GPIO_InitStructure->GPIO_Pin = GPIO_Pin_12 | GPIO_Pin_13| GPIO_Pin_14| GPIO_Pin_15;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure->GPIO_PuPd = GPIO_PuPd_NOPULL;
}
void initTim2(TIM_TimeBaseInitTypeDef* TIM_TimeBaseStructure)
{
	 TIM_TimeBaseStructure->TIM_Period = 999; //1999
	 TIM_TimeBaseStructure->TIM_Prescaler = 550 - 1;
	 TIM_TimeBaseStructure->TIM_ClockDivision = TIM_CKD_DIV1;
	 TIM_TimeBaseStructure->TIM_CounterMode = TIM_CounterMode_Up ;
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

void initSPI(SPI_InitTypeDef*  SPI_InitStructure)
{
	/* SPI configuration -------------------------------------------------------*/
	SPI_InitStructure->SPI_Direction = SPI_Direction_2Lines_FullDuplex;
	SPI_InitStructure->SPI_DataSize = SPI_DataSize_8b;
	SPI_InitStructure->SPI_CPOL = SPI_CPOL_Low;
	SPI_InitStructure->SPI_CPHA = SPI_CPHA_1Edge;
	SPI_InitStructure->SPI_NSS = SPI_NSS_Soft;
	SPI_InitStructure->SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_4;
	SPI_InitStructure->SPI_FirstBit = SPI_FirstBit_MSB;
	SPI_InitStructure->SPI_CRCPolynomial = 7;
	SPI_InitStructure->SPI_Mode = SPI_Mode_Master;
}

void configGPIOforListChip(GPIO_InitTypeDef* GPIO_InitStructure)
{
	/* Configure GPIO PIN for Lis Chip select */
	GPIO_InitStructure->GPIO_Pin = LIS302DL_SPI_CS_PIN;
	GPIO_InitStructure->GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure->GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure->GPIO_Speed = GPIO_Speed_50MHz;
	//GPIO_Init(LIS302DL_SPI_CS_GPIO_PORT, &GPIO_InitStructure);

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
