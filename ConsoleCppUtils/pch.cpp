// pch.cpp : fichier source correspondant à l'en-tête précompilé

#include "pch.h"

#include <windows.h>
#include <cwchar>

#define EXPORTED_METHODE extern "C" __declspec(dllexport)

HANDLE hStdin;
DWORD fdwSaveOldMode;

VOID ErrorExit(LPCSTR);
VOID KeyEventProc(KEY_EVENT_RECORD);
VOID MouseEventProc(MOUSE_EVENT_RECORD);
VOID ResizeEventProc(WINDOW_BUFFER_SIZE_RECORD);


//Console Utils
EXPORTED_METHODE
int SetConsoleFontSize(int s)
{
    CONSOLE_FONT_INFOEX cfi;
    cfi.cbSize = sizeof(cfi);
    cfi.nFont = 0;
    cfi.dwFontSize.X = s;                   // Width of each character in the font
    cfi.dwFontSize.Y = s * 2;                  // Height
    //cfi.FontFamily = FF_DONTCARE;
    cfi.FontWeight = FW_NORMAL;
    //std::wcscpy(cfi.FaceName, L"Consolas"); // Choose your font
    SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), FALSE, &cfi);
    return 0;
}

EXPORTED_METHODE
int BlockScrolling()
{
    ShowScrollBar(GetConsoleWindow(), SB_VERT, 0);
    return 0;
}

EXPORTED_METHODE
int SetConsoleToPixel()
{
    CONSOLE_FONT_INFOEX cfi;
    cfi.cbSize = sizeof(cfi);
    cfi.nFont = 0;
    cfi.dwFontSize.X = 0;                   // Width of each character in the font
    cfi.dwFontSize.Y = 1;                  // Height
    //cfi.FontFamily = FF_DONTCARE;
    cfi.FontWeight = FW_NORMAL;
    //std::wcscpy(cfi.FaceName, L"Consolas"); // Choose your font
    SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), FALSE, &cfi);
    return 0;
}