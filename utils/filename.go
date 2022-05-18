package utils

import (
	"path/filepath"
	"runtime"
)

func RelativeFile(filename string) string {
	_, currentFileName, _, ok := runtime.Caller(1)
	if !ok {
		return ""
	}
	dirname := filepath.Dir(currentFileName)

	return dirname + string(filepath.Separator) + filename
}
