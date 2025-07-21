cmake_minimum_required(VERSION 3.12)

project(nonewrap)

add_library(nonewrap SHARED
  dllmain.c nonewrap.c nonewrap.h)

target_compile_definitions(nonewrap PRIVATE NONEWRAP_EXPORTS)
if(WIN32)
  set_target_properties(nonewrap PROPERTIES PREFIX "")
endif()
