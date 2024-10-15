// Demo.cpp : Ce fichier contient la fonction 'main'. L'exécution du programme commence et se termine à cet endroit.
//

#include <iostream>
#include "ScipturesEngine.h"

using namespace scriptures;

int main()
{
	Window wd = Window()
		.SetSize(150, 35)
		.SetTitle("Wiremole");
	//wd.Write("Hello world", 10, 10);

	Tablet t = Tablet(&wd, 10, 10, 10, 10);

	Sleep(100);
	for (int i = 0; i < 10; i++)
	{
		for (int j = 0; j < 10; j++) {
			if (i % 2 == 1)
				t.Write("#", i, j);
			else
				t.Write("@", i, j, RED);
		}
	}
	int c = 0;
	while(c < 100000)
	{
		wd.Update();
		c++;
	}
	
	cout << "Done";
	Sleep(10000);
	return 0;
}