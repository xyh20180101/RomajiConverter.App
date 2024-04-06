# RomajiConverter.App

使用Avalonia框架开发的跨平台罗马音转换器

**该项目只针对Android平台进行开发与调试，也只有Android平台的Release**，对于其他平台：

Windouws 10/11：已有原生项目[RomajiConverter.WinUI](https://github.com/xyh20180101/RomajiConverter.WinUI)

Windows 7：已有原生项目[RomajiConverter](https://github.com/xyh20180101/RomajiConverter)

Linux/IOS：本项目框架支持，但没有在上面运行和测试过

**界面预览图**

![](/doc/preview.jpg)

## 功能

该项目由Windows项目迁移而来，详细功能介绍请跳转[RomajiConverter.WinUI](https://github.com/xyh20180101/RomajiConverter.WinUI)

### 缺少的功能
- 直接获取网易云音乐当前播放歌词
- 详细模式开关（App改为常开）
- 字体大小缩放
- 生成图片选项中的字体选择（Windows可选择已安装字体，App只有预设的两种：Sans-黑体，Serif-宋体）

### 增加的功能
- 输出文本框只读

## 说明

开发过程中发现实机运行性能不是很好，不知道是框架的问题还是代码优化的问题，不过功能是可以正常用的

安装包大小为170M，安装并运行一次后为应用242M+数据248M=共490M，不要清除数据因为那是必须的，以后可能会优化大小

## 下载
- [Release](https://github.com/xyh20180101/RomajiConverter.App/releases)

## 更新日志

### 0.1.1
- 修复保存歌词json时没有保存编辑结果的问题
- 优化打开歌词逻辑

### 0.1.0
- 迁移基本功能