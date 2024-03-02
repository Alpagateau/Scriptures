#include "pch.h"
#include "ipt.h"
#include <windows.h>
#include <stdio.h>
#include <tchar.h>
#include <iostream>

#define EXPORTED_METHODE extern "C" __declspec(dllexport)

struct mousePos
{
    int x;
    int y;
};

EXPORTED_METHODE
HANDLE dll_GetStandartHandle(int kind)
{
	if (kind == 0)
		return GetStdHandle(STD_INPUT_HANDLE);
	if (kind == 1)
		return GetStdHandle(STD_OUTPUT_HANDLE);
	if (kind == 2)
		return GetStdHandle(STD_ERROR_HANDLE);
}
/*
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
*/

EXPORTED_METHODE
void dll_getMousePos(mousePos *mp, HANDLE hdl)
{
    HANDLE InputHandle = hdl;
    INPUT_RECORD irInBuf[128];
    DWORD lenght, fdwMode;
    DWORD fdwSaveOldMode;

    if (!GetConsoleMode(InputHandle, &fdwSaveOldMode)) {
        std::cout << "ERROR : Line 32" << std::endl;
        return;
    }
    fdwMode = ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT | ENABLE_INSERT_MODE | ENABLE_EXTENDED_FLAGS;
    if (!SetConsoleMode(InputHandle, fdwMode)) {
        std::cout << "SetConsoleMode" << std::endl;
        return;
    }
    if (!PeekConsoleInput(
        InputHandle,      // input buffer handle 
        irInBuf,     // buffer to read into 
        128,         // size of read buffer 
        &lenght)) {// number of records read 
        std::cout << "ReadConsoleInput" << std::endl;
        SetConsoleMode(InputHandle, fdwSaveOldMode);
        return;
    }
    for (int i = 0; i < lenght; i++)
    {
        switch (irInBuf[i].EventType)
        {
        case MOUSE_EVENT: // mouse input 
            mp->x = irInBuf[i].Event.MouseEvent.dwMousePosition.X;
            mp->y = irInBuf[i].Event.MouseEvent.dwMousePosition.Y;
            break;
        default:
            break;
        }
    }
}

EXPORTED_METHODE
void dll_getPressedKeys(BYTE kpi[32], HANDLE hdl)
{
    HANDLE InputHandle = hdl;
    INPUT_RECORD irInBuf[128];
    DWORD lenght, fdwMode;
    DWORD fdwSaveOldMode;

    if (!GetConsoleMode(InputHandle, &fdwSaveOldMode)) {
        //std::cout << "ERROR : Line 32" << std::endl;
        throw std::invalid_argument("cant get console mode");
        return;
    }
    fdwMode = ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT | ENABLE_INSERT_MODE | ENABLE_EXTENDED_FLAGS;
    if (!SetConsoleMode(InputHandle, fdwMode)) {
        throw std::invalid_argument("cant set console mode");
        return;
    }
    DWORD value;
    LPDWORD lpdWord = &value;
    GetNumberOfConsoleInputEvents(InputHandle, lpdWord);
    if (value > 0){
        if (!PeekConsoleInput(
            InputHandle,      // input buffer handle 
            irInBuf,     // buffer to read into 
            128,         // size of read buffer 
            &lenght)) {// number of records read 
            std::cout << "ReadConsoleInput" << std::endl;
            SetConsoleMode(InputHandle, fdwSaveOldMode);
            return;
        }
        for (int i = 0; i < lenght; i++)
        {
            switch (irInBuf[i].EventType)
            {
            case KEY_EVENT: // mouse input
                //if key is down
                //get index
                unsigned int ui = irInBuf[i].Event.KeyEvent.wVirtualKeyCode;
                int bIndex = ui % 8;
                int byteIndex = (ui - bIndex) / 8;
                if (irInBuf[i].Event.KeyEvent.bKeyDown) {
                    kpi[byteIndex] |= (0x01 << bIndex);
                }
                else {
                    BYTE b = (0x01 << bIndex);
                    b = ~b;
                    kpi[byteIndex] &= b;
                }
                //std::cout << ui << " || ";
                break;
            }
        }
        //std::cout << std::endl;
    }
    if (value > 128)
    {
        FlushConsoleInputBuffer(InputHandle);
    }
}
