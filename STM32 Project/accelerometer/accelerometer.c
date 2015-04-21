#include "accelerometer.h"

__IO uint32_t  LIS302DLTimeout = LIS302DL_FLAG_TIMEOUT;
uint16_t PrescalerValue = 0;
__IO uint32_t TimingDelay = 0;

void LIS302DL_Init(LIS302DL_InitTypeDef *LIS302DL_InitStruct)
{
  uint8_t ctrl = 0x00;

  /* Configure the low level interface ---------------------------------------*/
//  LIS302DL_LowLevel_Init();

  /* Configure MEMS: data rate, power mode, full scale, self test and axes */
  ctrl = (uint8_t) (LIS302DL_InitStruct->Output_DataRate | LIS302DL_InitStruct->Power_Mode | \
                    LIS302DL_InitStruct->Full_Scale | LIS302DL_InitStruct->Self_Test | \
                    LIS302DL_InitStruct->Axes_Enable);

  /* Write value to MEMS CTRL_REG1 regsister */
  LIS302DL_Write(&ctrl, LIS302DL_CTRL_REG1_ADDR, 1);
}

void LIS302DL_Write(uint8_t* pBuffer, uint8_t WriteAddr, uint16_t NumByteToWrite)
{
  /* Configure the MS bit:
       - When 0, the address will remain unchanged in multiple read/write commands.
       - When 1, the address will be auto incremented in multiple read/write commands.
  */
  if(NumByteToWrite > 0x01)
  {
    WriteAddr |= (uint8_t)MULTIPLEBYTE_CMD;
  }
  /* Set chip select Low at the start of the transmission */
  LIS302DL_CS_LOW();

  /* Send the Address of the indexed register */
  LIS302DL_SendByte(WriteAddr);
  /* Send the data that will be written into the device (MSB First) */
  while(NumByteToWrite >= 0x01)
  {
    LIS302DL_SendByte(*pBuffer);
    NumByteToWrite--;
    pBuffer++;
  }

  /* Set chip select High at the end of the transmission */
  LIS302DL_CS_HIGH();
}

static uint8_t LIS302DL_SendByte(uint8_t byte)
{
  /* Loop while DR register in not emplty */
  LIS302DLTimeout = LIS302DL_FLAG_TIMEOUT;
  while (SPI_I2S_GetFlagStatus(LIS302DL_SPI, SPI_I2S_FLAG_TXE) == RESET)
  {
    if((LIS302DLTimeout--) == 0) return LIS302DL_TIMEOUT_UserCallback();
  }

  /* Send a Byte through the SPI peripheral */
  SPI_I2S_SendData(LIS302DL_SPI, byte);

  /* Wait to receive a Byte */
  LIS302DLTimeout = LIS302DL_FLAG_TIMEOUT;
  while (SPI_I2S_GetFlagStatus(LIS302DL_SPI, SPI_I2S_FLAG_RXNE) == RESET)
  {
    if((LIS302DLTimeout--) == 0) return LIS302DL_TIMEOUT_UserCallback();
  }

  /* Return the Byte read from the SPI bus */
  return (uint8_t)SPI_I2S_ReceiveData(LIS302DL_SPI);
}

void LIS302DL_InterruptConfig(LIS302DL_InterruptConfigTypeDef *LIS302DL_IntConfigStruct)
{
  uint8_t ctrl = 0x00;

  /* Read CLICK_CFG register */
  LIS302DL_Read(&ctrl, LIS302DL_CLICK_CFG_REG_ADDR, 1);

  /* Configure latch Interrupt request, click interrupts and double click interrupts */
  ctrl = (uint8_t)(LIS302DL_IntConfigStruct->Latch_Request| \
                   LIS302DL_IntConfigStruct->SingleClick_Axes | \
                   LIS302DL_IntConfigStruct->DoubleClick_Axes);

  /* Write value to MEMS CLICK_CFG register */
  LIS302DL_Write(&ctrl, LIS302DL_CLICK_CFG_REG_ADDR, 1);
}

void LIS302DL_Read(uint8_t* pBuffer, uint8_t ReadAddr, uint16_t NumByteToRead)
{
  if(NumByteToRead > 0x01)
  {
    ReadAddr |= (uint8_t)(READWRITE_CMD | MULTIPLEBYTE_CMD);
  }
  else
  {
    ReadAddr |= (uint8_t)READWRITE_CMD;
  }
  /* Set chip select Low at the start of the transmission */
  LIS302DL_CS_LOW();

  /* Send the Address of the indexed register */
  LIS302DL_SendByte(ReadAddr);

  /* Receive the data that will be read from the device (MSB First) */
  while(NumByteToRead > 0x00)
  {
    /* Send dummy byte (0x00) to generate the SPI clock to LIS302DL (Slave device) */
    *pBuffer = LIS302DL_SendByte(DUMMY_BYTE);
    NumByteToRead--;
    pBuffer++;
  }

  /* Set chip select High at the end of the transmission */
  LIS302DL_CS_HIGH();
}

void Delay(__IO uint32_t nCount)
{
  while(nCount--)
  {
  }
}

/**
  * @brief  Decrements the

Delay variable.
  * @param  None
  * @retval None
  */
void TimingDelay_Decrement(void)
{
  if (TimingDelay != 0x00)
  {
    TimingDelay--;
  }
}

/**
  * @brief  MEMS accelerometre management of the timeout situation.
  * @param  None.
  * @retval None.
  */
uint32_t LIS302DL_TIMEOUT_UserCallback(void)
{
  /* MEMS Accelerometer Timeout error occured */
  while (1)
  {
  }
}

#ifdef  USE_FULL_ASSERT

/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t* file, uint32_t line)
{
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */

  /* Infinite loop */
  while (1)
  {
  }
}
#endif

/**
  * @}
  */

/**
  * @}
  */

