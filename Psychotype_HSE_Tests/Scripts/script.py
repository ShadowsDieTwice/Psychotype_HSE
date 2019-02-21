print("heh")
file = open("testfile.txt", "a")

file.write("Hello World\n")
file.write("This is our new text file\n")
file.write(" and this is another line.\n")
file.write("Why? Because we can.\n")

file.close()

import sys
print(sys.executable)
