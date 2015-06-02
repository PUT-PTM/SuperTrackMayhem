/*#include "stm32f4xx_conf.h"
#include "stm32f4xx.h"
#include "stm32f4xx_gpio.h"
#include "stm32f4xx_rcc.h"
#include "stm32f4xx_exti.h"
#include "stm32f4xx_spi.h"

#include "usbd_cdc_core.h"
#include "usbd_usr.h"
#include "usbd_desc.h"
#include "usbd_cdc_vcp.h"
#include "usb_dcd_int.h"

#include <stdlib.h>
#include <stdio.h>

#include "misc.h"
#include "vcp.h"
#include "accelerometer.h"*/
#include "main.h"


int main(void)
{

    SystemInit();
    init();
    SystemCoreClockUpdate(); // inicjalizacja dystrybucji czasu procesora

    RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOA, ENABLE);//timer to button

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

    GPIO_PinAFConfig(LIS302DL_SPI_SCK_GPIO_PORT, LIS302DL_SPI_SCK_SOURCE, LIS302DL_SPI_SCK_AF);
    GPIO_PinAFConfig(LIS302DL_SPI_MISO_GPIO_PORT, LIS302DL_SPI_MISO_SOURCE, LIS302DL_SPI_MISO_AF);
    GPIO_PinAFConfig(LIS302DL_SPI_MOSI_GPIO_PORT, LIS302DL_SPI_MOSI_SOURCE, LIS302DL_SPI_MOSI_AF);

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
  //  uint8_t ctrl = 0x00;

    /* Set configuration of LIS302DL*/
    LIS302DL_InitStruct.Power_Mode = LIS302DL_LOWPOWERMODE_ACTIVE;
    LIS302DL_InitStruct.Output_DataRate = LIS302DL_DATARATE_100;
    LIS302DL_InitStruct.Axes_Enable = LIS302DL_X_ENABLE | LIS302DL_Y_ENABLE | LIS302DL_Z_ENABLE;
    LIS302DL_InitStruct.Full_Scale = LIS302DL_FULLSCALE_2_3;
    LIS302DL_InitStruct.Self_Test = LIS302DL_SELFTEST_NORMAL;
    LIS302DL_Init(&LIS302DL_InitStruct);

    LIS302DL_InterruptConfigTypeDef LIS302DL_InterruptStruct;
    /* Set configuration of Internal High Pass Filter of LIS302DL*/
    LIS302DL_InterruptStruct.Latch_Request = LIS302DL_INTERRUPTREQUEST_LATCHED;
    LIS302DL_InterruptStruct.SingleClick_Axes = LIS302DL_CLICKINTERRUPT_Z_ENABLE;
    LIS302DL_InterruptStruct.DoubleClick_Axes = LIS302DL_DOUBLECLICKINTERRUPT_Z_ENABLE;
    LIS302DL_InterruptConfig(&LIS302DL_InterruptStruct);

    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_0;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN;
    GPIO_Init(GPIOA, &GPIO_InitStructure);


    /*===========================================================*/
    int8_t przyspieszenie_x, przyspieszenie_y, przyspieszenie_z;
    int j;

    /*inicjalizacja pakietu akcelerometru*/
    accPacket_t accPacket;
    accPacket.start_flag=NEW_PACKET;
    accPacket.command=ACC_COMMAND_TYPE;

    /*inicjalizacja pakietu przycisku*/
    buttonPacket_t buttonPacket;
    buttonPacket.start_flag=NEW_PACKET;
    buttonPacket.command=BUTTON_COMMAND_TYPE;

    while(1)
    {

    	LIS302DL_Read(&przyspieszenie_x, LIS302DL_OUT_X_ADDR, 1);
    	if(przyspieszenie_x>127)
    	{
			przyspieszenie_x=przyspieszenie_x-1;
			przyspieszenie_x=(~przyspieszenie_x)&0xFF;
			przyspieszenie_x=-przyspieszenie_x;
    	}

        LIS302DL_Read(&przyspieszenie_y, LIS302DL_OUT_Y_ADDR, 1);
        if(przyspieszenie_y>127)
        {
            przyspieszenie_y=przyspieszenie_y-1;
            przyspieszenie_y=(~przyspieszenie_y)&0xFF;
            przyspieszenie_y=-przyspieszenie_y;
        }

        LIS302DL_Read(&przyspieszenie_z, LIS302DL_OUT_Z_ADDR, 1);
    	if(przyspieszenie_z>127)
        {
			przyspieszenie_z=przyspieszenie_z-1;
			przyspieszenie_z=(~przyspieszenie_z)&0xFF;
			przyspieszenie_z=-przyspieszenie_z;
        }

       for(j=0;j<300000;j++){  //slowloop
        }


        /*ustawienie wartosci pakietu dla akcelerometru*/
        accPacket.x=przyspieszenie_x;
        accPacket.y=((przyspieszenie_y+128)/128)*9.8;
        accPacket.z=przyspieszenie_z;
        accPacket.crc=CRC_START;

        /*wyslanie pakietu akcelerometru*/
        VCP_send_buffer(&accPacket, sizeof(accPacket_t));


        /*ustawienie wartosci dla pakietu przycisku*/
        if(GPIO_ReadInputDataBit(GPIOA, GPIO_Pin_0))
        	buttonPacket.butt1_state=BUTTON_CLICKED;
        else
        	buttonPacket.butt1_state=BUTTON_NO_CLICKED;
        buttonPacket.butt2_state=BUTTON_NO_CLICKED;
        buttonPacket.butt3_state=BUTTON_NO_CLICKED;
        buttonPacket.butt4_state=BUTTON_NO_CLICKED;
        buttonPacket.crc=CRC_START;
        /*wyslanie pakietu przycisku*/
        VCP_send_buffer(&buttonPacket, sizeof(buttonPacket_t));






    }
}

