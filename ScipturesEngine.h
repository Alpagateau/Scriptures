#pragma once

#include <string>
#include <iostream>
#include <Windows.h>
#include "keycodes.h"

#ifndef __SCRIPTURES_ENGINE_H__
#define __SCRIPTURES_ENGINE_H__

#ifndef SCRIPTURES_VERSION
#define SCRIPTURES_VERSION 1.1.0
#endif

#ifdef SCIPTURES_EXPORT
#define SCRIPTURES_API __declspec(dllexport)
#else
#define SCRIPTURES_API __declspec(dllimport)
#endif

using namespace std;
namespace scriptures {

	enum SCRIPTURES_API ConsoleColor {
		BLACK = 0x0,
		BLUE = 0x1,
		GREEN = 0x2,
		BLUE_GREY = 0x3,
		RED = 0x4,
		PURPLE = 0x5,
		YELLOW = 0x6,
		WHITE = 0x7,
		GREY = 0x8,
		LITE_BLUE = 0x9,
		LITE_GREEN = 0xA,
		CYAN = 0xB,
		LITE_RED = 0xC,
		LITE_PURPLE = 0xD,
		LITE_YELLOW = 0xE,
		SHINING_WHITE = 0xF
	};

	class SCRIPTURES_API Window;

	class SCRIPTURES_API InputHandler
	{
	public:
		InputHandler();
		void Initialize(Window* w);

		void ReadInputs();
		bool GetKeyDown(KeyCode kc);
		bool GetKey(KeyCode kc);
	private:
		void CheckInit();
		Window* pWindow;

		unsigned char fInput[32]; //Inputs down on this frame
		unsigned char pInput[32]; //Inputs held from other frames

	};

	class SCRIPTURES_API Window
	{
	public:
		Window();
		Window SetSize(int w, int h);
		Window SetTitle(string title);

		void SetCursorPosition(int x, int y);
		COORD GetCursorPosition();
		HANDLE GetConsoleHandle(bool input);

		BOOL _SetConsoleSize(int cols, int rows);
		//Write
		void Write(string txt, int x, int y, ConsoleColor foreground, ConsoleColor background);
		void Write(string txt, int x, int y, ConsoleColor foreground);
		void Write(string txt, int x, int y);
		void Write(string txt);

		void Write(string txt, ConsoleColor foreground, ConsoleColor background);
		void Write(string txt, ConsoleColor foreground);

		void Clear();
		void ClearLine();
		void ClearLine(int y);

		//Game Loops
		void Update();
		scriptures::InputHandler _inputHandler;

	private:
		int width = 0;
		int height = 0;
		int pWidth = 0;
		int pHeight = 0;
		HWND console;
		HANDLE hInput;
		HANDLE hOutput;
		
	};

	class SCRIPTURES_API Tablet // a subscreen basically
	{
	public:
		Tablet(Window* window, int x, int y, int w, int h);

		void SetCursorPosition(int x, int y);

		//Write
		void Write(string txt, int x, int y, ConsoleColor foreground, ConsoleColor background);
		void Write(string txt, int x, int y, ConsoleColor foreground);
		void Write(string txt, int x, int y);
		void Write(string txt);

		void Write(string txt, ConsoleColor foreground, ConsoleColor background);
		void Write(string txt, ConsoleColor foreground);

		void Clear();
		void ClearLine();
		void ClearLine(int y);
		
	private:
		Window* pWindow;
		int xpos = 0;
		int ypos = 0;
		int width = 0;
		int height = 0;
	};
	
}	

#endif
