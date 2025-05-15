#!/bin/bash
# Script to add HealthOverride property to Pokémon classes

# Path to Pokémon classes directory
POKEMON_DIR="c:/Users/yangx/OneDrive/Desktop/Feat Pokemon/Feat-Pokemon/server/Models/Pokemons"

# Process each Pokémon class file
for file in "$POKEMON_DIR"/*.cs; do
  echo "Processing $file..."
  
  # Check if file already has HealthOverride property
  if grep -q "HealthOverride" "$file"; then
    echo "  HealthOverride already exists, skipping..."
    continue
  fi
  
  # Extract the base HP value from the constructor
  BASE_HP=$(grep -o "base(.*)" "$file" | head -1 | sed -E 's/.*"[^"]+", "[^"]+", ([0-9]+).*/\1/')
  
  if [ -z "$BASE_HP" ]; then
    echo "  Could not find HP value, skipping..."
    continue
  fi
  
  # Create temporary file
  TMP_FILE=$(mktemp)
  
  # Modified approach: find the position right after the opening brace of the class
  awk -v hp="$BASE_HP" '
    /public class .* : PokemonMaster/ {print; inclass=1; next}
    inclass && /^{/ {print; print "    public override float HealthOverride {get;set;} = " hp ";"; inclass=0; next}
    inclass && /{/ {print; print "    public override float HealthOverride {get;set;} = " hp ";"; inclass=0; next}
    {print}
  ' "$file" > "$TMP_FILE"
  
  # Replace original with modified file
  mv "$TMP_FILE" "$file"
  
  echo "  Added HealthOverride = $BASE_HP to $(basename "$file")"
done

echo "Done processing all Pokémon files!"