cmake_minimum_required(VERSION 3.12)

project(zstdwrap)

add_library(zstdwrap SHARED
  dllmain.c zstdwrap.c zstdwrap.h)

include(FetchContent)

set(ZSTD_BUILD_STATIC ON)
set(ZSTD_BUILD_SHARED OFF)

FetchContent_Declare(
  zstd
  DOWNLOAD_EXTRACT_TIMESTAMP ON
  URL https://github.com/facebook/zstd/releases/download/v1.5.7/zstd-1.5.7.tar.gz
  URL_HASH SHA256=eb33e51f49a15e023950cd7825ca74a4a2b43db8354825ac24fc1b7ee09e6fa3
  SOURCE_SUBDIR build/cmake
  EXCLUDE_FROM_ALL
)

FetchContent_MakeAvailable(zstd)

target_include_directories(zstdwrap PRIVATE ${zstd_SOURCE_DIR}/lib)
target_compile_definitions(zstdwrap PRIVATE ZSTDWRAP_EXPORTS ZSTDLIB_STATIC_API)
if(MSVC)
  target_compile_options(zstdwrap PRIVATE /MT)
else()
  target_link_options(zstdwrap PRIVATE -static)
endif()
target_link_libraries(zstdwrap PRIVATE libzstd_static)
if(WIN32)
  set_target_properties(zstdwrap PROPERTIES PREFIX "")
endif()
