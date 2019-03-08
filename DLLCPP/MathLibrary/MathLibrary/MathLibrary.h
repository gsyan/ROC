#pragma once

//MATHLIBRARY_EXPORTS 가 Preprocessor Definitions 에 추가 되있기 때문에 윗줄 활성
#ifdef MATHLIBRARY_EXPORTS
#define MATHLIBRARY_API __declspec(dllexport)
#else
#define MATHLIBRARY_API __declspec(dllimport)
#endif

// This function must be called before any other function.
extern "C" MATHLIBRARY_API void Fibonacci_init(const unsigned long long a, const unsigned long long b);

// Produce the next value in the sequence.
// Returns true on success and updates current value and index;
// false on overflow, leaves current value and index unchanged.
extern "C" MATHLIBRARY_API bool Fibonacci_next();

// Get the current value in the sequence.
extern "C" MATHLIBRARY_API unsigned long long Fibonacci_current();

// Get the position of the current value in the sequence.
extern "C" MATHLIBRARY_API unsigned Fibonacci_index();
