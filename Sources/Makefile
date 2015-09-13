SRC = $(shell sed -n 's:\s*<Compile Include="\([^"]*\)" />:Front-end/\1:p' \
        Front-end/Front-end.fsproj | grep -v 'GUI.fs' )

DDIR = Front-end/bin/Debug/
RDIR = Front-end/bin/Release/

all: debug

debug: $(SRC)
	mkdir -p $(DDIR)
	cp -R -u Content/Content $(DDIR)
	fsharpc -o $(DDIR)DeductionEngine.exe --debug:full -g --optimize- --tailcalls+\
		--platform:"x64" --nologo $(SRC)

release: $(SRC)
	mkdir -p $(RDIR)
	cp -R -u Content/Content $(RDIR)
	fsharpc -o $(RDIR)DeductionEngine.exe --debug:pbonly --optimize+ --tailcalls+ \
		--crossoptimize+ --platform:"x64" --nologo $(SRC)

clean:
	rm -Rf $(shell sed 's:^/::' .gitignore)

