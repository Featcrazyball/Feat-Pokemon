#!/bin/bash
# Script to generate additional constructors for Pokemon classes
# Usage: ./generate_constructors.sh <directory_path>

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
    CONSTRUCTOR=$(sed -n '/public '$CLASS_NAME'\(string nickname, string ownerId\)/,/}/p' "$file")
    
    if [ -z "$CONSTRUCTOR" ]; then
        echo "Skipping $file - couldn't find appropriate constructor"
        continue
    fi
    
    # Extract the base constructor parameters
    BASE_PARAMS=$(echo "$CONSTRUCTOR" | grep -o ": base(.*)" | sed 's/: base(//' | sed 's/)//')
    
    # Split parameters and replace the third one (HP) with 100
    IFS=',' read -r -a PARAMS <<< "$BASE_PARAMS"
    if [ ${#PARAMS[@]} -lt 3 ]; then
        echo "Skipping $file - not enough parameters in base constructor"
        continue
    fi
    
    # Get the SkillPool
    SKILL_POOL=$(echo "$CONSTRUCTOR" | grep -o 'SkillPool = ".*";' | sed 's/SkillPool = //' | sed 's/;//')
    
    # Create the new constructor
    NEW_CONSTRUCTOR="
    public $CLASS_NAME(string ownerId)
    : base(${PARAMS[0]}, ${PARAMS[1]}, 100, ${PARAMS[3]}, ${PARAMS[4]}, ${PARAMS[5]}, ${PARAMS[6]}, ${PARAMS[7]}, ownerId, ${PARAMS[9]}, ${PARAMS[10]})
    {
        Experience = 0;
        Nickname = \"None\";
        SkillPool = $SKILL_POOL;

        var newSkills = LearnSkillFromSkillPool();
        if (newSkills != null)
        {
            foreach (var skill in newSkills)
            {
                Skills.Add(skill);
            }
        }
    }"
    
    # Insert the new constructor after the existing one
    INSERTION_POINT=$(grep -n "public $CLASS_NAME(string nickname, string ownerId)" "$file" | cut -d: -f1)
    INSERTION_POINT=$(awk -v line="$INSERTION_POINT" 'NR==line{i=1} i&&/}/{print NR; exit}' "$file")
    
    if [ -z "$INSERTION_POINT" ]; then
        echo "Skipping $file - couldn't find insertion point"
        continue
    fi
    
    sed -i "${INSERTION_POINT}a\\${NEW_CONSTRUCTOR}" "$file"
    echo "Added new constructor to $file"
done

echo "Constructor generation complete!"