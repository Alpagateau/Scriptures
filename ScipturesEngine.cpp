#include "pch.h"
#include <iostream>
#include <string>
#include "ScipturesEngine.h"
#include <Windows.h>
#include <iomanip>
#include "keycodes.h"

namespace scriptures
{
#pragma region Window
	Window::Window()
	{
		console = GetConsoleWindow();
		hInput = GetStdHandle(STD_INPUT_HANDLE);
		hOutput = GetStdHandle(STD_OUTPUT_HANDLE);
		DWORD mode;
		GetConsoleMode(hOutput, &mode);
		SetConsoleMode(console, mode);

		SetWindowLong(console, GWL_STYLE, GetWindowLong(console, GWL_STYLE) & ~WS_MAXIMIZEBOX & ~WS_SIZEBOX);
		//_inputHandler = InputHandler();
		_inputHandler.Initialize(this);
	}

	Window Window::SetSize(int w, int h)
	{	
		width = w;
		height = h;

		cout << _SetConsoleSize(w, h);

		return *this;
	}

	Window Window::SetTitle(string title)
	{
		LPCSTR t = title.c_str();
		SetConsoleTitleA(t);
		return *this;
	}

	void Window::SetCursorPosition(int x, int y)
	{
		if (x < 0 || x >= width)
			throw std::invalid_argument("X is out of range");
		if (y < 0 || y >= height)
			throw std::invalid_argument("Y is out of range");
		COORD pos = { x, y };
		SetConsoleCursorPosition(hOutput, pos);
	}

	COORD Window::GetCursorPosition()
	{
		CONSOLE_SCREEN_BUFFER_INFO csbi;

		GetConsoleScreenBufferInfo(hOutput, &csbi);

		return csbi.dwCursorPosition;
	}

	void Window::Write(string txt, int x, int y, ConsoleColor foreground, ConsoleColor background)
	{
		SetConsoleTextAttribute(hOutput, (background << 4) | foreground);
		if (width - (txt.size() + y) > 0)
		{
			this->SetCursorPosition(x, y);
			cout << txt;
		}
		else
		{
			int i = 0;
			int px = x;
			int py = y;
			for (; i < txt.length(); i++)
			{
				this->SetCursorPosition(px, py);
				cout << txt[i];
				px++;
				if (px > width + 1)
				{
					px = 0;
					py++;
				}
			}
		}
		SetConsoleTextAttribute(hOutput, (BLACK << 4) | WHITE);
	}

	void Window::Write(string txt, ConsoleColor foreground, ConsoleColor background)
	{
		COORD a = GetCursorPosition();
		Write(txt, a.X, a.Y, foreground, background);
	}

	void Window::Write(string txt, int x, int y, ConsoleColor foreground)
	{
		Write(txt, x, y, foreground, ConsoleColor::BLACK);
	}

	void Window::Write(string txt, ConsoleColor foreground)
	{
		COORD a = GetCursorPosition();
		Write(txt, a.X, a.Y, foreground, ConsoleColor::BLACK);
	}

	void Window::Write(string txt, int x, int y)
	{
		Write(txt, x, y, ConsoleColor::WHITE, ConsoleColor::BLACK);
	}

	void Window::Write(string txt)
	{
		COORD a = GetCursorPosition();
		Write(txt, a.X, a.Y, ConsoleColor::WHITE, ConsoleColor::BLACK);
	}

	HANDLE Window::GetConsoleHandle(bool inp)
	{
		return  inp ? hInput : hOutput;
	}

	void Window::Clear()
	{
		system("cls");
	}

	void Window::ClearLine()
	{
		COORD a = GetCursorPosition();
		Write(" ", 0, a.Y);
		for (int i = 0; i < width - 1; i++)
		{
			cout << " ";
		}
		SetCursorPosition(a.X, a.Y);
	}

	void Window::ClearLine(int y)
	{
		Write(" ", 0, y);
		for (int i = 0; i < width - 1; i++)
		{
			cout << " ";
		}
		SetCursorPosition(0, y);
	}

	void Window::Update()
	{
		_inputHandler.ReadInputs();
	}

	BOOL Window::_SetConsoleSize(int cols, int rows) {
		HWND hWnd;
		HANDLE hConOut;
		CONSOLE_FONT_INFO fi;
		CONSOLE_SCREEN_BUFFER_INFO bi;
		int w, h, bw, bh;
		RECT rect = {0, 0, 0, 0};
		COORD coord = {0, 0};
		hWnd = GetConsoleWindow();
		if (hWnd) {
			hConOut = GetStdHandle(STD_OUTPUT_HANDLE);
			if (hConOut && hConOut != (HANDLE)-1) {
			if (GetCurrentConsoleFont(hConOut, FALSE, &fi)) {
				if (GetClientRect(hWnd, &rect)) {
				w = rect.right-rect.left;
				h = rect.bottom-rect.top;
				if (GetWindowRect(hWnd, &rect)) {
					bw = rect.right-rect.left-w;
					bh = rect.bottom-rect.top-h;
					if (GetConsoleScreenBufferInfo(hConOut, &bi)) {
					coord.X = bi.dwSize.X;
					coord.Y = bi.dwSize.Y;
					if (coord.X < cols || coord.Y < rows) {
						if (coord.X < cols) {
						coord.X = cols;
						}
						if (coord.Y < rows) {
						coord.Y = rows;
						}
						
					}
					coord.X = cols;
					coord.Y = rows;

					if (!SetConsoleScreenBufferSize(hConOut, coord)) {
						return FALSE;
					}
					return SetWindowPos(
                hWnd, 
                NULL, 
                rect.left, 
                rect.top, 
                cols*fi.dwFontSize.X+bw, 
                rows*fi.dwFontSize.Y+bh, 
                SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER | SWP_NOZORDER);
					}
				}
				}
			}
			}
		}
		return FALSE;
	}

#pragma endregion

#pragma region Tablets

	Tablet::Tablet(Window* window, int x, int y, int w, int h)
	{
		pWindow = window;
		xpos = x;
		ypos = y;

		width = w;
		height = h;
	}

	void Tablet::SetCursorPosition(int x, int y)
	{
		if (x < 0 || x >= width)
			throw std::invalid_argument("X is out of range");
		if (y < 0 || y >= height)
			throw std::invalid_argument("Y is out of range");
		pWindow->SetCursorPosition(x + xpos, y + ypos);
	}

	void Tablet::Write(string txt, int x, int y, ConsoleColor foreground, ConsoleColor background)
	{
		pWindow->Write(txt, x + xpos, y + ypos, foreground, background);
	}

	void Tablet::Write(string txt, int x, int y, ConsoleColor foreground)
	{
		Write(txt, x, y, foreground, ConsoleColor::BLACK);
	}

	void Tablet::Write(string txt, int x, int y)
	{
		Write(txt, x, y, ConsoleColor::WHITE, ConsoleColor::BLACK);
	}

	void Tablet::Write(string txt)
	{
		Write(txt, 0, 0, ConsoleColor::WHITE, ConsoleColor::BLACK);
	}

	void Tablet::Write(string txt, ConsoleColor foreground, ConsoleColor background)
	{
		pWindow->Write(txt, foreground, background);
	}

	void Tablet::Write(string txt, ConsoleColor foreground)
	{
		Write(txt, foreground, ConsoleColor::BLACK);
	}

	void Tablet::Clear()
	{
		for (int i = 0; i < height; i++)
		{
			ClearLine(i);
		}
	}

	void Tablet::ClearLine()
	{
		COORD a = pWindow->GetCursorPosition();
		Write(" ", 0, a.Y);
		for (int i = 0; i < width - 1; i++)
		{
			cout << " ";
		}
		SetCursorPosition(0, a.Y);
	}

	void Tablet::ClearLine(int y)
	{
		Write(" ", 0, y);
		for (int i = 0; i < width - 1; i++)
		{
			cout << " ";
		}
		SetCursorPosition(0, y);
	}
#pragma endregion

#pragma region InputHandler

	InputHandler::InputHandler()
	{
		cout << "Input handler created\n";
		pWindow = NULL;
	}

	void InputHandler::Initialize(Window* w)
	{
		pWindow = w;
		cout << "Input Handler Initialized to window";
		int len = sizeof(fInput) / sizeof(fInput[0]);
		for (int i = 0; i < len; i++)
		{
			fInput[i] = 0x00;
			pInput[i] = 0x00;
		}
	}

	void InputHandler::CheckInit()
	{
		if (pWindow == NULL)
		{
			throw new std::exception("You have to initialize input handlers");
		}
	}

	void InputHandler::ReadInputs()
	{
		DWORD fdwSaveOldMode;

		DWORD cNumRead, fdwMode, i;
		INPUT_RECORD irInBuf[128];
		int counter = 0;

		// Save the current input mode, to be restored on exit.
		if (!GetConsoleMode(pWindow->GetConsoleHandle(true), &fdwSaveOldMode))
			cout << "GetConsoleMode";

		// Enable the window and mouse input events.
		fdwMode = ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT;
		if (!SetConsoleMode(pWindow->GetConsoleHandle(true), fdwMode))
			cout << "SetConsoleMode";

		// Loop to read and handle the next 100 input events.
		if (!PeekConsoleInput(
			pWindow->GetConsoleHandle(true),      // input buffer handle
			irInBuf,     // buffer to read into
			128,         // size of read buffer
			&cNumRead)) // number of records read
			cout << "ReadConsoleInput";

		if (cNumRead > 0)
			ReadConsoleInput(pWindow->GetConsoleHandle(true), irInBuf, 128, &cNumRead);

		int len = sizeof(fInput) / sizeof(fInput[0]);
		for (int i = 0; i < len; i++)
		{
			fInput[i] = 0;
		}
    
    // Updates the key buffer

		for (i = 0; i < cNumRead; i++)
		{
			switch (irInBuf[i].EventType)
			{
			case KEY_EVENT: // keyboard input //Set in fInput
			{
				int idx = irInBuf[i].Event.KeyEvent.wVirtualKeyCode;
				int sec = idx / 8;
				int lstIdx = idx - (sec * 8);

				if (irInBuf[i].Event.KeyEvent.bKeyDown) {
					fInput[sec] |= (1 << lstIdx);
					pInput[sec] |= (1 << lstIdx);
				}
				else
				{
					pInput[sec] &= ~(1 << lstIdx);
				}
			}
			break;

			case MOUSE_EVENT: // mouse input
				//TODO
				break;
      case WINDOW_BUFFER_SIZE_EVENT:
        //TODO
        break;
			default:
				//ErrorExit("Unknown event type");
				break;
			}
		}
		// Restore input mode on exit.
		SetConsoleMode(pWindow->GetConsoleHandle(true), fdwSaveOldMode);
	}

	bool InputHandler::GetKey(KeyCode kc)
	{
		int v = (int)kc;
		int idx = v;
		int sec = idx / 8;
		int lstIdx = idx - (sec * 8);

		int h = (pInput[sec] >> lstIdx) & 1;
		return h == 1;
	}

	bool InputHandler::GetKeyDown(KeyCode kc)
	{
		int v = (int)kc;
		int idx = v;
		int sec = idx / 8;
		int lstIdx = idx - (sec * 8);

		int h = (fInput[sec] >> lstIdx) & 1;
		return h == 1;
	}


#pragma endregion
}
