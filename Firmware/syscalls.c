/*
 * syscalls.c
 *
 *  Created on: 05.12.2013
 *      Author: Andre
 */

#include <errno.h>
#include <sys/stat.h>
#include <sys/times.h>
#include <sys/unistd.h>
#include "hw_config.h"

#undef errno
extern int errno;

#ifdef __cplusplus
extern "C" {
#endif

/** Handler for pure virtual function calls.
 * This should be included, otherwise the exception handling is pulled in. */
void __cxa_pure_virtual()
{
	while (1);
}

int _write(int file, char *ptr, int len);

int _close(int file)
{
  return -1;
}

int _fstat(int file, struct stat *st)
{
  st->st_mode = S_IFCHR;
  return 0;
}


int _isatty(int file)
{
switch (file)
  {
    case STDOUT_FILENO:
    case STDERR_FILENO:
    case STDIN_FILENO:
      return 1;
    default:
      //errno = ENOTTY;
      errno = EBADF;
      return 0;
  }
}

int _lseek(int file, int ptr, int dir)
{
  return 0;
}

caddr_t _sbrk(int incr)
{
  extern char _end; // Defined by the linker
  static char *heap_end;
  char *prev_heap_end;

  if (heap_end == 0)
  {
    heap_end = &_end;
  }
  prev_heap_end = heap_end;

  char * stack = (char*) __get_MSP();
  if (heap_end + incr > stack)
  {
    _write(STDERR_FILENO, (char*)"Heap and stack collision\n", 25);
    errno = ENOMEM;
    return (caddr_t) -1;
    //abort ();
  }

  heap_end += incr;
  return (caddr_t) prev_heap_end;
}

//int _read(int file, char *ptr, int len)
//{
//  return 0;
//}
//
//int _write(int file, char *ptr, int len)
//{
//  return len;
//}

#ifdef __cplusplus
}
#endif
