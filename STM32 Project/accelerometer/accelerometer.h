#ifndef ACCELEROMETER_H
#define ACCELEROMETER_H
#include <stdlib.h>
#include <stdio.h>
#include "stm32f4xx.h"
#include "stm32f4xx_gpio.h"
#include "stm32f4xx_rcc.h"
#include "misc.h"
#include "stm32f4xx_spi.h"

extern __IO uint32_t  LIS302DLTimeout;
extern uint16_t PrescalerValue;
extern __IO uint32_t TimingDelay;

/* LIS302DL struct */
typedef struct
{
  uint8_t Power_Mode;                         /* Power-down/Active Mode */
  uint8_t Output_DataRate;                    /* OUT data rate 100 Hz / 400 Hz */
  uint8_t Axes_Enable;                        /* Axes enable */
  uint8_t Full_Scale;                         /* Full scale */
  uint8_t Self_Test;                          /* Self test */
}LIS302DL_InitTypeDef;

/* LIS302DL High Pass Filter struct */
typedef struct
{
  uint8_t HighPassFilter_Data_Selection;      /* Internal filter bypassed or data from internal filter send to output register*/
  uint8_t HighPassFilter_CutOff_Frequency;    /* High pass filter cut-off frequency */
  uint8_t HighPassFilter_Interrupt;           /* High pass filter enabled for Freefall/WakeUp #1 or #2 */
}LIS302DL_FilterConfigTypeDef;

/* LIS302DL Interrupt struct */
typedef struct
{
  uint8_t Latch_Request;                      /* Latch interrupt request into CLICK_SRC register*/
  uint8_t SingleClick_Axes;                   /* Single Click Axes Interrupts */
  uint8_t DoubleClick_Axes;                   /* Double Click Axes Interrupts */
}LIS302DL_InterruptConfigTypeDef;


#define LIS302DL_FLAG_TIMEOUT         ((uint32_t)0x1000)
#define LIS302DL_SPI                       SPI1
#define LIS302DL_SPI_CLK                   RCC_APB2Periph_SPI1

#define LIS302DL_SPI_SCK_PIN               GPIO_Pin_5                  /* PA.05 */
#define LIS302DL_SPI_SCK_GPIO_PORT         GPIOA                       /* GPIOA */
#define LIS302DL_SPI_SCK_GPIO_CLK          RCC_AHB1Periph_GPIOA
#define LIS302DL_SPI_SCK_SOURCE            GPIO_PinSource5
#define LIS302DL_SPI_SCK_AF                GPIO_AF_SPI1

#define LIS302DL_SPI_MISO_PIN              GPIO_Pin_6                  /* PA.6 */
#define LIS302DL_SPI_MISO_GPIO_PORT        GPIOA                       /* GPIOA */
#define LIS302DL_SPI_MISO_GPIO_CLK         RCC_AHB1Periph_GPIOA
#define LIS302DL_SPI_MISO_SOURCE           GPIO_PinSource6
#define LIS302DL_SPI_MISO_AF               GPIO_AF_SPI1

#define LIS302DL_SPI_MOSI_PIN              GPIO_Pin_7                  /* PA.7 */
#define LIS302DL_SPI_MOSI_GPIO_PORT        GPIOA                       /* GPIOA */
#define LIS302DL_SPI_MOSI_GPIO_CLK         RCC_AHB1Periph_GPIOA
#define LIS302DL_SPI_MOSI_SOURCE           GPIO_PinSource7
#define LIS302DL_SPI_MOSI_AF               GPIO_AF_SPI1

#define LIS302DL_SPI_CS_PIN                GPIO_Pin_3                  /* PE.03 */
#define LIS302DL_SPI_CS_GPIO_PORT          GPIOE                       /* GPIOE */
#define LIS302DL_SPI_CS_GPIO_CLK           RCC_AHB1Periph_GPIOE

#define LIS302DL_SPI_INT1_PIN              GPIO_Pin_0                  /* PE.00 */
#define LIS302DL_SPI_INT1_GPIO_PORT        GPIOE                       /* GPIOE */
#define LIS302DL_SPI_INT1_GPIO_CLK         RCC_AHB1Periph_GPIOE
#define LIS302DL_SPI_INT1_EXTI_LINE        EXTI_Line0
#define LIS302DL_SPI_INT1_EXTI_PORT_SOURCE EXTI_PortSourceGPIOE
#define LIS302DL_SPI_INT1_EXTI_PIN_SOURCE  EXTI_PinSource0
#define LIS302DL_SPI_INT1_EXTI_IRQn        EXTI0_IRQn

#define LIS302DL_SPI_INT2_PIN              GPIO_Pin_1                  /* PE.01 */
#define LIS302DL_SPI_INT2_GPIO_PORT        GPIOE                       /* GPIOE */
#define LIS302DL_SPI_INT2_GPIO_CLK         RCC_AHB1Periph_GPIOE
#define LIS302DL_SPI_INT2_EXTI_LINE        EXTI_Line1
#define LIS302DL_SPI_INT2_EXTI_PORT_SOURCE EXTI_PortSourceGPIOE
#define LIS302DL_SPI_INT2_EXTI_PIN_SOURCE  EXTI_PinSource1
#define LIS302DL_SPI_INT2_EXTI_IRQn        EXTI1_IRQn


#define LIS302DL_WHO_AM_I_ADDR                  0x0F
#define LIS302DL_CTRL_REG1_ADDR                 0x20
#define LIS302DL_CTRL_REG2_ADDR              0x21
#define LIS302DL_CTRL_REG3_ADDR              0x22
#define LIS302DL_HP_FILTER_RESET_REG_ADDR     0x23
#define LIS302DL_STATUS_REG_ADDR             0x27
#define LIS302DL_OUT_X_ADDR                  0x29
#define LIS302DL_OUT_Y_ADDR                  0x2B
#define LIS302DL_OUT_Z_ADDR                  0x2D
#define LIS302DL_FF_WU_CFG1_REG_ADDR         0x30
#define LIS302DL_FF_WU_SRC1_REG_ADDR           0x31
#define LIS302DL_FF_WU_THS1_REG_ADDR          0x32
#define LIS302DL_FF_WU_DURATION1_REG_ADDR     0x33
#define LIS302DL_FF_WU_CFG2_REG_ADDR         0x34
#define LIS302DL_FF_WU_SRC2_REG_ADDR           0x35
#define LIS302DL_FF_WU_THS2_REG_ADDR          0x36
#define LIS302DL_FF_WU_DURATION2_REG_ADDR     0x37
#define LIS302DL_CLICK_CFG_REG_ADDR     0x38
#define LIS302DL_CLICK_SRC_REG_ADDR        0x39
#define LIS302DL_CLICK_THSY_X_REG_ADDR        0x3B
#define LIS302DL_CLICK_THSZ_REG_ADDR         0x3C
#define LIS302DL_CLICK_TIMELIMIT_REG_ADDR        0x3D
#define LIS302DL_CLICK_LATENCY_REG_ADDR        0x3E
#define LIS302DL_CLICK_WINDOW_REG_ADDR        0x3F
#define LIS302DL_SENSITIVITY_2_3G                         18  /* 18 mg/digit*/
#define LIS302DL_SENSITIVITY_9_2G                         72  /* 72 mg/digit*/
#define LIS302DL_DATARATE_100                             ((uint8_t)0x00)
#define LIS302DL_DATARATE_400                             ((uint8_t)0x80)
#define LIS302DL_LOWPOWERMODE_POWERDOWN                   ((uint8_t)0x00)
#define LIS302DL_LOWPOWERMODE_ACTIVE                      ((uint8_t)0x40)
#define LIS302DL_FULLSCALE_2_3                            ((uint8_t)0x00)
#define LIS302DL_FULLSCALE_9_2                            ((uint8_t)0x20)
#define LIS302DL_SELFTEST_NORMAL                          ((uint8_t)0x00)
#define LIS302DL_SELFTEST_P                               ((uint8_t)0x10)
#define LIS302DL_SELFTEST_M                               ((uint8_t)0x08)
#define LIS302DL_X_ENABLE                                 ((uint8_t)0x01)
#define LIS302DL_Y_ENABLE                                 ((uint8_t)0x02)
#define LIS302DL_Z_ENABLE                                 ((uint8_t)0x04)
#define LIS302DL_XYZ_ENABLE                               ((uint8_t)0x07)
#define LIS302DL_SERIALINTERFACE_4WIRE                    ((uint8_t)0x00)
#define LIS302DL_SERIALINTERFACE_3WIRE                    ((uint8_t)0x80)
#define LIS302DL_BOOT_NORMALMODE                          ((uint8_t)0x00)
#define LIS302DL_BOOT_REBOOTMEMORY                        ((uint8_t)0x40)
#define LIS302DL_FILTEREDDATASELECTION_BYPASSED           ((uint8_t)0x00)
#define LIS302DL_FILTEREDDATASELECTION_OUTPUTREGISTER     ((uint8_t)0x20)
#define LIS302DL_HIGHPASSFILTERINTERRUPT_OFF              ((uint8_t)0x00)
#define LIS302DL_HIGHPASSFILTERINTERRUPT_1                ((uint8_t)0x04)
#define LIS302DL_HIGHPASSFILTERINTERRUPT_2                ((uint8_t)0x08)
#define LIS302DL_HIGHPASSFILTERINTERRUPT_1_2              ((uint8_t)0x0C)
#define LIS302DL_HIGHPASSFILTER_LEVEL_0                   ((uint8_t)0x00)
#define LIS302DL_HIGHPASSFILTER_LEVEL_1                   ((uint8_t)0x01)
#define LIS302DL_HIGHPASSFILTER_LEVEL_2                   ((uint8_t)0x02)
#define LIS302DL_HIGHPASSFILTER_LEVEL_3                   ((uint8_t)0x03)
#define LIS302DL_INTERRUPTREQUEST_NOTLATCHED              ((uint8_t)0x00)
#define LIS302DL_INTERRUPTREQUEST_LATCHED                 ((uint8_t)0x40)
#define LIS302DL_CLICKINTERRUPT_XYZ_DISABLE               ((uint8_t)0x00)
#define LIS302DL_CLICKINTERRUPT_X_ENABLE                  ((uint8_t)0x01)
#define LIS302DL_CLICKINTERRUPT_Y_ENABLE                  ((uint8_t)0x04)
#define LIS302DL_CLICKINTERRUPT_Z_ENABLE                  ((uint8_t)0x10)
#define LIS302DL_CLICKINTERRUPT_XYZ_ENABLE                ((uint8_t)0x15)
#define LIS302DL_DOUBLECLICKINTERRUPT_XYZ_DISABLE         ((uint8_t)0x00)
#define LIS302DL_DOUBLECLICKINTERRUPT_X_ENABLE            ((uint8_t)0x02)
#define LIS302DL_DOUBLECLICKINTERRUPT_Y_ENABLE            ((uint8_t)0x08)
#define LIS302DL_DOUBLECLICKINTERRUPT_Z_ENABLE            ((uint8_t)0x20)
#define LIS302DL_DOUBLECLICKINTERRUPT_XYZ_ENABLE          ((uint8_t)0x2A)
#define LIS302DL_CS_LOW()       GPIO_ResetBits(LIS302DL_SPI_CS_GPIO_PORT, LIS302DL_SPI_CS_PIN)
#define LIS302DL_CS_HIGH()      GPIO_SetBits(LIS302DL_SPI_CS_GPIO_PORT, LIS302DL_SPI_CS_PIN)
#define READWRITE_CMD              ((uint8_t)0x80)
#define MULTIPLEBYTE_CMD           ((uint8_t)0x40)
#define DUMMY_BYTE                 ((uint8_t)0x00)

void Delay(__IO uint32_t nCount);
void LIS302DL_Init(LIS302DL_InitTypeDef *LIS302DL_InitStruct);
void LIS302DL_Write(uint8_t* pBuffer, uint8_t WriteAddr, uint16_t NumByteToWrite);
static uint8_t LIS302DL_SendByte(uint8_t byte);
void LIS302DL_InterruptConfig(LIS302DL_InterruptConfigTypeDef *LIS302DL_IntConfigStruct);
void LIS302DL_Read(uint8_t* pBuffer, uint8_t ReadAddr, uint16_t NumByteToRead);
uint32_t LIS302DL_TIMEOUT_UserCallback(void);

#endif
