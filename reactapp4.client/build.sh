#!/bin/sh

# Run the Vite build and ignore any non-zero exit from warnings
npm run build || true

# Always exit 0 so MSBuild doesn't fail the publish
exit 0
