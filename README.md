# Touchpad Manager
Auto enable/disable touch-pad according to other pointing devices availability.

# Why this?
Windows (10 at least) has it as a standard feature. Just use it instead. But if you happen to have an ASUS laptop as me,
its touch-pad may not be recognized as a touch-pad. (It may be technically classified as a pointing device of unknown kind).
Then only the vendor specific programs may allow you auto-enabled/disabled it according to other pointing devices availability.

As, in my case, those vendor specific programs were grabbing way too much CPU for what they provide me, I have removed them.
And so I am needing another way to get that touch-pad disabled when I have a mouse plugged-in.

# Releases
This source is not release ready. This is [Work on my machine](https://blog.codinghorror.com/the-works-on-my-machine-certification-program/)
software.
So just grab the source, compile it and check if it can be of any usage to you.

# Testing
This is a windows service. For easing testing, you can run it as a command line program by adding `console` as an argument.
```
TouchpadManager.exe console
```
Better use an elevated command line (run it in admin mode), otherwise it will not be allowed to change the touch-pad device state.

# Installing
I have put that in `c:\Program Files\TouchpadManager\` then within an elevated VS developer prompt run from that directory:
```
installutil TouchpadManager.Exe
```