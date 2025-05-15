#!/bin/bash
# Script to add a parameterized constructor to each PokemonMaster subclass

# Path to Pokémon classes directory
POKEMON_DIR="c:/Users/yangx/OneDrive/Desktop/Feat Pokemon/Feat-Pokemon/server/Models/Pokemons"

# Process each Pokémon class file
for file in "$POKEMON_DIR"/*.cs; do
  echo "Processing $file..."
  
  # Extract the class name from the file
  CLASS_NAME=$(grep -o "public class [A-Za-z]* : PokemonMaster" "$file" | awk '{print $3}')
  if [ -z "$CLASS_NAME" ]; then
    echo "  Could not find class name in $file, skipping..."
    continue
  fi
  
  # Check if file already has the HP constructor
  if grep -q "public $CLASS_NAME(float HP, string nickname, string ownerId, int exp)" "$file"; then
    echo "  HP constructor already exists in $CLASS_NAME, fixing if needed..."
    
    # Fix missing comma after HP
    sed -i 's/: base("\([^"]*\)", "\([^"]*\)", HP \([0-9]\)/: base("\1", "\2", HP, \3/g' "$file"
    echo "  Fixed constructor if needed"
    continue
  fi
  
  # Extract the base constructor call parameters
  BASE_PARAMS=$(grep -o ": base(.*)" "$file" | head -1 | sed 's/: base(//' | sed 's/)//')
  if [ -z "$BASE_PARAMS" ]; then
    echo "  Could not find base constructor parameters in $file, skipping..."
    continue
  fi
  
  # Extract the name, type and all parameters in order
  NAME=$(echo "$BASE_PARAMS" | awk -F', ' '{print $1}')
  TYPE=$(echo "$BASE_PARAMS" | awk -F', ' '{print $2}')
  HP=$(echo "$BASE_PARAMS" | awk -F', ' '{print $3}')
  ATTACK=$(echo "$BASE_PARAMS" | awk -F', ' '{print $4}')
  DEFENSE=$(echo "$BASE_PARAMS" | awk -F', ' '{print $5}')
  SP_ATTACK=$(echo "$BASE_PARAMS" | awk -F', ' '{print $6}')
  SP_DEFENSE=$(echo "$BASE_PARAMS" | awk -F', ' '{print $7}')
  SPEED=$(echo "$BASE_PARAMS" | awk -F', ' '{print $8}')
  OWNER_ID=$(echo "$BASE_PARAMS" | awk -F', ' '{print $9}')
  SKILL_DAMAGE=$(echo "$BASE_PARAMS" | awk -F', ' '{print $10}')
  ABILITY=$(echo "$BASE_PARAMS" | awk -F', ' '{print $11}')
  
  # Extract the SkillPool assignment from the first constructor
  SKILL_POOL=$(grep -o "SkillPool = \".*\";" "$file" | head -1)
  if [ -z "$SKILL_POOL" ]; then
    echo "  Could not find SkillPool in $file, will use empty skillpool..."
    SKILL_POOL="SkillPool = \"\";"
  fi
  
  # Create the new constructor with comma after HP
  NEW_CONSTRUCTOR=$(cat <<EOF

    public $CLASS_NAME(float HP, string nickname, string ownerId, int exp)
    : base($NAME, $TYPE, HP, $ATTACK, $DEFENSE, $SP_ATTACK, $SP_DEFENSE, $SPEED, ownerId, $SKILL_DAMAGE, $ABILITY)
    {
        Nickname = nickname;
        Experience = exp;
        $SKILL_POOL

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills) 
            {
                Skills.Add(skill);
            };
        }
    }
EOF
)
  
  # Create temporary file
  TMP_FILE=$(mktemp)
  
  # Find the end of the first constructor to insert our new constructor there
  awk -v class="$CLASS_NAME" -v new_constructor="$NEW_CONSTRUCTOR" '
    BEGIN { in_constructor = 0; brace_count = 0; }
    /public '"$CLASS_NAME"'\(string nickname, string ownerId\)/ { in_constructor = 1; }
    in_constructor && /{/ { brace_count++; }
    in_constructor && /}/ { 
      brace_count--;
      if (brace_count == 0) {
        in_constructor = 0;
        print $0;
        print new_constructor;
        next;
      }
    }
    { print; }
  ' "$file" > "$TMP_FILE"
  
  # Replace original file with modified one
  mv "$TMP_FILE" "$file"
  
  echo "  Added HP constructor to $CLASS_NAME"
done

# Fix all existing constructors with missing commas
for file in "$POKEMON_DIR"/*.cs; do
  sed -i 's/: base("\([^"]*\)", "\([^"]*\)", HP \([0-9]\)/: base("\1", "\2", HP, \3/g' "$file"
done

echo "All files processed and constructors fixed!"