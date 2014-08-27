https://github.com/amccorma/xamarin-amccorma

For now, only Android. But working up an IOS version.

entry = new MyEntry ();
entry.Text = "";
entry.Keyboard = Keyboard.Numeric;
entry.FormatCharacters = "-";
	
Format Characters: (FormatCharacters)
"-" : dash
"-(/" : multiple characters
	
Rules definition:

new MaskRules {  Start = 0, End = 3, Mask = "" }
new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"}
new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"}
new MaskRules { Start = 10, End = 20, Mask = "{0:}"}

			
Mask Format:
{0:3}:  take the first 3 characters
{3:3}:  start at character 3 and take 3 characters
{0:}:   apply no mask.  take all the characters and output them
{6:3}:  start at character 6 and take 3 characters


Final mask:
999-999-9999 if less than 10 characters
if 10 characters to 20, use {0:}. just output the text

			
Text Length from 0 to 3 characters. apply the Mask "".  Mask = "", no formatting
new MaskRules {  Start = 0, End = 3, Mask = "" }


Text Length from 0 to 3 characters. apply the Mask "{0:3}-{3:}".
{0:3}: first 3 characters
followed by a "-"
next three characters {3:0}
new MaskRules { Start = 4, End = 6, Mask = "{0:3}-{3:}"}


Text Length from 0 to 3 characters. apply the Mask "{0:3}-{3:}".
{0:3}: first 3 characters
followed by a "-"
next three characters {3:0}
add a "-"
next characters use (6:0}
new MaskRules { Start = 7, End = 10, Mask = "{0:3}-{3:3}-{6:}"}


Text Length from 10 to 20 characters. 
20 characters MAX Length
apply the Mask "{0:} is the same as no Mask.  
new MaskRules { Start = 10, End = 20, Mask = "{0:}"}

