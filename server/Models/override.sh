#!/bin/bash
# Script to generate additional constructors for Pokemon classes
# Uses Abra as a reference pattern

DIR_PATH=${1:-"C:/Users/yangx/OneDrive/Desktop/Pokemon/Feat-Pokemon/server/Models/Pokemons"}
POKEMON_FILES=$(find "$DIR_PATH" -name "*.cs" -type f)

for file in $POKEMON_FILES; do
    # Skip files that already have the single parameter constructor
    if grep -q "public .*(string ownerId)" "$file"; then
        echo "Skipping $file - already has single parameter constructor"
        continue
    fi
    
    # Get the class name
    CLASS_NAME=$(grep -o "public class [A-Za-z0-9]* : PokemonMaster" "$file" | awk '{print $3}')
    
    if [ -z "$CLASS_NAME" ]; then
        echo "Skipping $file - couldn't find class name"
        continue
    fi
    
    echo "Processing $CLASS_NAME in file $file"
    
    # Find the constructor with nickname and ownerId
    CONSTRUCTOR=$(grep -A20 "public $CLASS_NAME *( *string nickname, string ownerId *)" "$file")
    
    if [ -z "$CONSTRUCTOR" ]; then
        echo "Skipping $file - couldn't find appropriate constructor"
        continue
    fi
    
    # Extract the base constructor parameters
    BASE_PARAMS=$(echo "$CONSTRUCTOR" | grep -o ": base(.*)" | sed 's/: base(//' | sed 's/)//')
    
    # Get the Pokemon name and type (first two parameters)
    NAME=$(echo "$BASE_PARAMS" | cut -d',' -f1)
    TYPE=$(echo "$BASE_PARAMS" | cut -d',' -f2)
    
    # Get the remaining parameters after the 3rd one (which will be replaced with 100)
    REST_PARAMS=$(echo "$BASE_PARAMS" | cut -d',' -f4- | sed 's/^[ \t]*//')
    
    # Get the SkillPool - protect from quote issues
    SKILL_POOL=$(grep -o 'SkillPool = ".*";' "$file" | head -1 | sed 's/SkillPool = //' | sed 's/;$//')
    
    # Write the new constructor to a temp file to avoid sed escaping issues
    cat > temp_constructor.txt << EOF
    
    public $CLASS_NAME(string ownerId) 
    : base($NAME, $TYPE, 100, $REST_PARAMS)
    {
        Experience = 0;
        Nickname = "None";
        SkillPool = $SKILL_POOL;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
        }
    }
EOF

    # Find the end of the first constructor block
    FIRST_CONSTRUCTOR=$(grep -n "public $CLASS_NAME *( *string nickname, string ownerId *)" "$file" | head -1 | cut -d: -f1)
    if [ -z "$FIRST_CONSTRUCTOR" ]; then
        echo "Skipping $file - couldn't find constructor"
        continue
    fi
    
    # Find the closing brace of this constructor
    END_LINE=$((FIRST_CONSTRUCTOR + 50))  # Look within the next 50 lines
    CONSTRUCTOR_END=$(head -n $END_LINE "$file" | tail -n $((END_LINE - FIRST_CONSTRUCTOR)) | grep -n "    }" | head -1 | cut -d: -f1)
    
    if [ -z "$CONSTRUCTOR_END" ]; then
        echo "Skipping $file - couldn't find constructor end point"
        continue
    fi
    
    # Calculate the actual line number
    CONSTRUCTOR_END=$((FIRST_CONSTRUCTOR + CONSTRUCTOR_END - 1))
    
    # Insert the new constructor after the first constructor block
    sed -i "${CONSTRUCTOR_END}r temp_constructor.txt" "$file"
    echo "Added new constructor to $file"
done

# Clean up
rm -f temp_constructor.txt
echo "Constructor generation complete!"