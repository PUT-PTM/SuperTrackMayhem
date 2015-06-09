#ifndef MAIN_H
#define MAIN_H

#include "stm32f4xx_conf.h"
#include "stm32f4xx_exti.h"
#include "usbd_cdc_core.h"
#include "usbd_usr.h"
#include "usbd_desc.h"
#include "usbd_cdc_vcp.h"
#include "usb_dcd_int.h"

#include "misc.h"
#include "vcp.h"

#include "ucp.h"

int main(void);
void enableClocks(void);
void gpio_PinAFConfig(void);
void initTim2(TIM_TimeBaseInitTypeDef* TIM_TimeBaseStructure);
void initLeds(GPIO_InitTypeDef* GPIO_InitStructure);
void initButton(GPIO_InitTypeDef* GPIO_InitStructure);
void initLIS302DL(LIS302DL_InitTypeDef*  LIS302DL_InitStruct, LIS302DL_InterruptConfigTypeDef* LIS302DL_InterruptStruct);
void initSPI(SPI_InitTypeDef*  SPI_InitStructure);
void configGPIOforListChip(GPIO_InitTypeDef* GPIO_InitStructure);
#endif
