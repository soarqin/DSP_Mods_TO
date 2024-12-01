# CompressSave

#### Compress game saves to reduce space use and boost save speed
#### Original by [@bluedoom](https://github.com/bluedoom/DSP_Mod)(till 1.1.11) and [@starfi5h](https://github.com/starfi5h/DSP_CompressSave)(1.1.12), I just update it to support latest game version.
#### 压缩游戏存档以降低空间使用并提升保存速度
#### 原作者 [@bluedoom](https://github.com/bluedoom/DSP_Mod)(直到1.1.11) 和 [@starfi5h](https://github.com/starfi5h/DSP_CompressSave)(1.1.12)，本人继续更新以支持最新游戏版本。

## Introduction

* Reduce archive size by 30% / save time by 75% (Compressed by LZ4, on HDD + i7-4790K@4.4G + DDR3 2400MHz)

  | Before | After |
  | - | - |
  | 50MB 0.8s | 30MB 0.2s |
  | 220M 3.6s | 147M 0.7s |
  | 1010M 19.1s | 690M 3.6s |

## Usage

* You can set compression type for manual saves and auto saves individually.
* Manual saves are compressed while using the new `Save` button.
* You can still get speed benefit while setting compression type to `None` for auto saves, and for manual saves if using the new `Save` button.
* You can decompress saves on load panel.
* Remember to backup your save(use original save button) before updating game to avoid loading failure.

## 介绍

* 减少存档容量30% / 存档用时75% (LZ4压缩，测试环境：机械硬盘 + i7-4790K@4.4G + DDR3 2400MHz)  

  | 原存档 | 压缩后 |
  | - | - |
  | 50MB 0.8s | 30MB 0.2s |
  | 220M 3.6s | 147M 0.7s |
  | 1010M 19.1s | 690M 3.6s |

## 使用说明

* 手动和自动存档都可以分开设置压缩方式。
* 手动存档使用新加的保存按钮即可压缩保存。
* 即使设置为不压缩，自动存档、以及使用新加的保存按钮手动保存也可以获得速度提升。
* 可以在读取存档面板解压存档。
* 如果游戏有版本更新记得先备份存档(使用原保存按钮)以免更新后无法读取存档。
