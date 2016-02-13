/**
 * Linker script for Cortex-M0 processors
 *
 */

MEMORY
{
    FLASH        (rx)  : ORIGIN = 0x08000000, LENGTH = 64K
    RAM          (rwx) : ORIGIN = 0x20000000, LENGTH = 16K
}

_estack = 0x20000000 + LENGTH(RAM);

SECTIONS
{
    .text : 
    {
    	_stext = .;
    	__region_ROM_start__ = _stext;
        KEEP(*(.isr_vector .isr_vector.*))
        *(.text .text.*)
        *(.gnu.linkonce.t.*)
        *(.glue_7)
        *(.glue_7t)
        *(.gcc_except_table)
        *(.rodata .rodata*)
        *(.gnu.linkonce.r.*)
        
        __cmd_tbl_start = .;
        KEEP(*(SORT(.cmd_tbl.*)))  /* shell command table */
        __cmd_tbl_end = .;

	    . = ALIGN(4);
	    KEEP(*(.init))

	    . = ALIGN(4);
	    __preinit_array_start = .;
	    KEEP (*(.preinit_array))
	    __preinit_array_end = .;

	    . = ALIGN(4);
	    __init_array_start = .;
	    KEEP (*(SORT(.init_array.*)))
	    KEEP (*(.init_array))
    	__init_array_end = .;

    	. = ALIGN(4);
	    KEEP(*(.fini))

	    . = ALIGN(4);
    	__fini_array_start = .;
	    KEEP (*(.fini_array))
	  	KEEP (*(SORT(.fini_array.*)))
	    __fini_array_end = .;

	    . = ALIGN(4);
    	KEEP (*crtbegin.o(.ctors))
    	KEEP (*(EXCLUDE_FILE(*crtend.o) .ctors))
    	KEEP (*(SORT(.ctors.*)))
    	KEEP (*crtend.o(.ctors))
    	KEEP (*crtbegin.o(.dtors))
    	KEEP (*(EXCLUDE_FILE (*crtend.o) .dtors))
    	KEEP (*(SORT(.dtors.*)))
    	KEEP (*crtend.o(.dtors))
        
        _etext = .;
        __region_ROM_end__ = _etext;
    } >FLASH
    
	.ARM.extab : 
	{
		*(.ARM.extab* .gnu.linkonce.armextab.*)
	} > FLASH

	__exidx_start = .;
	.ARM.exidx :
	{
		*(.ARM.exidx* .gnu.linkonce.armexidx.*)
	} > FLASH
	__exidx_end = .;

    .text.align :
    {
        . = ALIGN(4);
        _sidata = .;
    } > FLASH
	    
    .data : AT ( _sidata)
    {
        _sdata = .;
        *(vtable vtable.*)
        *(.data .data.*)
        *(.gnu.linkonce.d*)
        . = ALIGN(4);
        
		/* preinit data */
		PROVIDE_HIDDEN (__preinit_array_start = .);
		KEEP(*(.preinit_array))
		PROVIDE_HIDDEN (__preinit_array_end = .);

		. = ALIGN(4);
		/* init data */
		PROVIDE_HIDDEN (__init_array_start = .);
		KEEP(*(SORT(.init_array.*)))
		KEEP(*(.init_array))
		PROVIDE_HIDDEN (__init_array_end = .);


		. = ALIGN(4);
		/* finit data */
		PROVIDE_HIDDEN (__fini_array_start = .);
		KEEP(*(SORT(.fini_array.*)))
		KEEP(*(.fini_array))
		PROVIDE_HIDDEN (__fini_array_end = .);
		        
        _edata = . ;
    } >RAM

    .bss : 
    {
        _sbss = . ;
        *(.bss .bss.*)
        *(.gnu.linkonce.b*)
        *(COMMON)
        . = ALIGN(4);
        _ebss = . ;
    } >RAM

    .noinit (NOLOAD) :
    {
        _snoinit = .;
        *(.noinit*)
        _enoinit = .;
        . = ALIGN(4);
    } >RAM

    _end = . ;
    PROVIDE (end = .);
    
    /* Stabs debugging sections.  */
    .stab          0 : { *(.stab) }
    .stabstr       0 : { *(.stabstr) }
    .stab.excl     0 : { *(.stab.excl) }
    .stab.exclstr  0 : { *(.stab.exclstr) }
    .stab.index    0 : { *(.stab.index) }
    .stab.indexstr 0 : { *(.stab.indexstr) }
    .comment       0 : { *(.comment) }
    /* DWARF debug sections.
       Symbols in the DWARF debugging sections are relative to the beginning
       of the section so we begin them at 0.  */
    /* DWARF 1 */
    .debug          0 : { *(.debug) }
    .line           0 : { *(.line) }
    /* GNU DWARF 1 extensions */
    .debug_srcinfo  0 : { *(.debug_srcinfo) }
    .debug_sfnames  0 : { *(.debug_sfnames) }
    /* DWARF 1.1 and DWARF 2 */
    .debug_aranges  0 : { *(.debug_aranges) }
    .debug_pubnames 0 : { *(.debug_pubnames) }
    /* DWARF 2 */
    .debug_info     0 : { *(.debug_info .gnu.linkonce.wi.*) }
    .debug_abbrev   0 : { *(.debug_abbrev) }
    .debug_line     0 : { *(.debug_line) }
    .debug_frame    0 : { *(.debug_frame) }
    .debug_str      0 : { *(.debug_str) }
    .debug_loc      0 : { *(.debug_loc) }
    .debug_macinfo  0 : { *(.debug_macinfo) }
    /* SGI/MIPS DWARF 2 extensions */
    .debug_weaknames 0 : { *(.debug_weaknames) }
    .debug_funcnames 0 : { *(.debug_funcnames) }
    .debug_typenames 0 : { *(.debug_typenames) }
    .debug_varnames  0 : { *(.debug_varnames) }
}
