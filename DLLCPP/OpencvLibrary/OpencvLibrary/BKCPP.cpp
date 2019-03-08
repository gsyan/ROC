#include <Windows.h>

#include "BKHeader.h"

#include <opencv2/core.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
using namespace cv;

#include <iostream>
#include <stdio.h>
using namespace std;

struct Color32
{
	byte r;
	byte g;
	byte b;
	byte a;
};



extern "C"
{
	OcvAPI void ProcessImage(Color32* raw, int width, int height)
	{
		
		Mat frame(height, width, CV_8UC4, raw);

		// check if we succeeded
		if (frame.empty()) {
			cerr << "ERROR! blank frame grabbed\n";
			return;
		}
		
		Mat frameOut;
		flip(frame, frameOut, 0);
		
		imshow("frame", frameOut);
	}

	OcvAPI BOOL LoadDll()
	{
		BOOL bTemp = FALSE;
		HMODULE hDll = LoadLibrary(L"opencv_world401.dll");
		if (NULL == hDll)
		{
			bTemp = FALSE;
		}
		else
		{
			FreeLibrary(hDll);
			bTemp = TRUE;
		}
		return bTemp;
	}

	OcvAPI LPCTSTR StringEX(LPCTSTR str)
	{
		return str;
	}


	OcvAPI bool JustCapturing()
	{
		//--- INITIALIZE VIDEOCAPTURE
		VideoCapture vc;

		int deviceID = 0;				// 0 = open default camera
		int apiID = cv::CAP_ANY;		// 0 = autodetect default API
		vc.open(deviceID + apiID);		// vc.open(0); 으로 3줄을 퉁칠수 있다. 그러나 명시적인 코드가 좋으므로

		if (!vc.isOpened())
		{
			cerr << "ERROR! Unable to open camera\n"; return false;
		}

		//--- GRAB AND WRITE LOOP
		cout << "Start grabbing" << endl
			<< "Press any key to terminate" << endl;


		Mat frame;
		while (true)
		{
			// wait for a new frame from camera and store it into 'frame'
			vc.read(frame);//vc >> frame; 로도 가능
			// check if we succeeded
			if (frame.empty()) {
				cerr << "ERROR! blank frame grabbed\n";
				break;
			}
			// show live and wait for a key with timeout long enough to show images
			imshow("Live", frame);
			if (waitKey(5) >= 0)
				break;

		}

		// the camera will be deinitialized automatically in VideoCapture destructor
		return true;
	}


}
