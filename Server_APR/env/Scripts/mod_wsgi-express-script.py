#!c:\users\deithdhu\documents\github\ai-photoriddle\server_apr\env\scripts\python.exe
# EASY-INSTALL-ENTRY-SCRIPT: 'mod-wsgi==4.7.1','console_scripts','mod_wsgi-express'
__requires__ = 'mod-wsgi==4.7.1'
import re
import sys
from pkg_resources import load_entry_point

if __name__ == '__main__':
    sys.argv[0] = re.sub(r'(-script\.pyw?|\.exe)?$', '', sys.argv[0])
    sys.exit(
        load_entry_point('mod-wsgi==4.7.1', 'console_scripts', 'mod_wsgi-express')()
    )