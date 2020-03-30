DIA:="dia"
ASCIIDOCTOR:="asciidoctor"
OPEN:="open"
UNAME=$(shell uname -o)
ifeq ($(UNAME),Cygwin)
	DIA:=$(shell cygpath "C:/Program Files (x86)/Dia/bin/dia.exe")
	ASCIIDOCTOR:="asciidoctor.bat"
	OPEN:="cygstart"
endif

DOCS=$(wildcard *.adoc)
OUTPUTS=$(DOCS:.adoc=.html)
AVAILABLE_DIA_SVGS:=$(patsubst %.dia,diag-%.svg,$(wildcard *.dia))


.PHONY: default
default: view


diag-%.svg: %.dia
	"$(DIA)" -e "$@" -t svg "$<"


%.html: %.adoc $(AVAILABLE_DIA_SVGS)
	"$(ASCIIDOCTOR)" -r asciidoctor-diagram "$<"


.PHONY: view
view: $(OUTPUTS)
	$(foreach output,$(OUTPUTS),"$(OPEN)" "$(output)";)


.PHONY: test-env
test-env: 
	echo "DIA=$(DIA)"
	echo "ASCIIDOCTOR=$(ASCIIDOCTOR)"
	echo "UNAME=$(UNAME)"


clean:
	-rm diag-*.png
	-rm diag-*.svg
	-rm $(OUTPUTS)
