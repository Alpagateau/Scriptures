#include "pch.h"
#include <windows.h>
#include <stdio.h>
#include <tchar.h>
#include <iostream>

#define EXPORTED_METHODE extern "C" __declspec(dllexport)

VOID KeyEventProc(KEY_EVENT_RECORD);
VOID MouseEventProc(MOUSE_EVENT_RECORD);
VOID ResizeEventProc(WINDOW_BUFFER_SIZE_RECORD);

EXPORTED_METHODE
HANDLE GetStandartHandle(int kind)
{
	if (kind == 0)
		return GetStdHandle(STD_INPUT_HANDLE);
	if (kind == 1)
		return GetStdHandle(STD_OUTPUT_HANDLE);
	if (kind == 2)
		return GetStdHandle(STD_ERROR_HANDLE);
}

EXPORTED_METHODE
int TestWithInputs()
{
	HANDLE InputHandle = GetStandartHandle(0);
    INPUT_RECORD irInBuf[128];
	DWORD lenght, fdwMode;

    DWORD fdwSaveOldMode;

    if (!GetConsoleMode(InputHandle, &fdwSaveOldMode)) {
        std::cout << "ERROR : Line 32" << std::endl;
        goto stop;
    }
    fdwMode = ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT | ENABLE_INSERT_MODE | ENABLE_EXTENDED_FLAGS;
    if (!SetConsoleMode(InputHandle, fdwMode)) {
        std::cout << "SetConsoleMode" << std::endl;
        goto stop;
    }
    if (!ReadConsoleInput(
        InputHandle,      // input buffer handle 
        irInBuf,     // buffer to read into 
        128,         // size of read buffer 
        &lenght)) {// number of records read 
        std::cout << "ReadConsoleInput" << std::endl;
        goto stop;
    }
    for (int i = 0; i < lenght; i++)
    {
        switch (irInBuf[i].EventType)
        {
        case KEY_EVENT: // keyboard input 
            KeyEventProc(irInBuf[i].Event.KeyEvent);
            break;

        case MOUSE_EVENT: // mouse input 
            MouseEventProc(irInBuf[i].Event.MouseEvent);
            break;

        case WINDOW_BUFFER_SIZE_EVENT: // scrn buf. resizing 
            ResizeEventProc(irInBuf[i].Event.WindowBufferSizeEvent);
            break;

        case FOCUS_EVENT:  // disregard focus events 

        case MENU_EVENT:   // disregard menu events 
            break;

        default:
            std::cout << "Unknown event type" << std::endl;
            break;
        }
    }

    stop: SetConsoleMode(InputHandle, fdwSaveOldMode);
	return -1;
}


VOID KeyEventProc(KEY_EVENT_RECORD ker)
{
    printf("Key event: ");

    if (ker.bKeyDown)
        printf("key pressed\n");
    else printf("key released\n");
}

VOID MouseEventProc(MOUSE_EVENT_RECORD mer)
{
#ifndef MOUSE_HWHEELED
#define MOUSE_HWHEELED 0x0008
#endif
    printf("Mouse event: ");

    switch (mer.dwEventFlags)
    {
    case 0:

        if (mer.dwButtonState == FROM_LEFT_1ST_BUTTON_PRESSED)
        {
            printf("left button press \n");
        }
        else if (mer.dwButtonState == RIGHTMOST_BUTTON_PRESSED)
        {
            printf("right button press \n");
        }
        else
        {
            printf("button press\n");
        }
        break;
    case DOUBLE_CLICK:
        printf("double click\n");
        break;
    case MOUSE_HWHEELED:
        printf("horizontal mouse wheel\n");
        break;
    case MOUSE_MOVED:
        printf("mouse moved\n");
        break;
    case MOUSE_WHEELED:
        printf("vertical mouse wheel\n");
        break;
    default:
        printf("unknown\n");
        break;
    }
}

VOID ResizeEventProc(WINDOW_BUFFER_SIZE_RECORD wbsr)
{
    printf("Resize event\n");
    printf("Console screen buffer is %d columns by %d rows.\n", wbsr.dwSize.X, wbsr.dwSize.Y);
}