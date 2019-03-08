#pragma once

#ifdef OPENCVLIBRARY_EXPORTS
#define OcvAPI __declspec(dllexport)
#else
#define OcvAPI __declspec(dllimport)
#endif



