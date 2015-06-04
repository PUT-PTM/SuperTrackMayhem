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

#endif
