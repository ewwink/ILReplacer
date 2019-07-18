

# ILReplacer (.Net Patcher)

**ILReplacer** is a Patcher or simple tool to find and replace .Net application IL code instructions such as `ldstr, ldc.i4, brfalse.s, etc..`  It support managed and mixed mode assembly.

Currently, this tool only check if instructions operand is valid or not if the `OpCode` is `Call` else not checked.

## Download
Download ILReplacer from [Release](https://github.com/ewwink/ILReplacer//releases) page, the package also contains block file `DemoTarget.ilr` and `DemoTarget.exe` for testing.

## Docs
 **The Blocks**
The blocks instruction format is like the following

    OpCode operand
    OpCode
    =

The symbol `=` is used in `Replace` input, it mean do not change original Opcode and Operand.

**BLocks Example**:

Change App name for DemoTarget.exe from `Demo App` to `Demo App || Cracked`

In `Find` textbox add:

    ldarg.0
    ldstr "Demo App"

In `Replace` textbox add

    ldarg.0
    ldstr "Demo App || Cracked"

or use `=` to keep `ldarg.0` unchanged

    =
    ldstr "Demo App || Cracked"

 **Multiple Blocks**
For replacing multiple blocks use the `=======` (minimum 3 characters) as separator

    ldarg.0
    ldstr "Demo App"
    ===========
    ldarg.0
    ldstr Other String

Note: the double quote in operand is optional

 **Find Blocks**
 If operand is not empty it will be used to compare with original operand, for example:
 
    call badMethod

will search if Operand **contains** method name `badMethod`

 **Replace Blocks**
 If the OpCode is a `Call` the operand have to be Hex number or MDToken like `0x06000001` or `06000001` and not method Name, example

    call 0x06000001

To know MDToken for method you can use [DnSpy](https://github.com/0xd4d/dnSpy) 

**Save and Load Blocks**
You can save and load the blocks to file, just click the **Menu** on the top.

### Screenshot
![ilreplacer](https://user-images.githubusercontent.com/760764/61352933-c67bac80-a898-11e9-8402-cf84f949ad90.jpg)

![ilreplacer-log](https://user-images.githubusercontent.com/760764/61352944-caa7ca00-a898-11e9-96ec-c70e9c5a8c93.jpg)

### Credit
Thanks to @0xd4d for dnlib library.
