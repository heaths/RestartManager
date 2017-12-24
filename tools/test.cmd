@if not defined _echo echo off

@REM Copyright (c) 2017 Heath Stewart
@REM See the LICENSE.txt file in the project root for more information.

powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "%~dp0\test.ps1" %*
