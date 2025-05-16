#!/bin/bash
# Script to update parameters in AdminGetPokemon method

# Path to User.cs file
USER_FILE="c:/Users/yangx/OneDrive/Desktop/Feat Pokemon/Feat-Pokemon/server/Models/User.cs"

# Create a temporary file
TMP_FILE=$(mktemp)

# Initialize variables to track if we're in AdminGetPokemon method
IN_ADMIN_METHOD=0
FINISHED_SWITCH=0

# Process the file line by line
while IFS= read -r line; do
    # Check if we're entering the AdminGetPokemon method
    if [[ $line =~ "public PokemonMaster? AdminGetPokemon" ]]; then
        IN_ADMIN_METHOD=1
    fi
    
    # Check if we're starting the switch statement in AdminGetPokemon
    if [[ $IN_ADMIN_METHOD -eq 1 && $line =~ "switch" ]]; then
        FINISHED_SWITCH=0
    fi
    
    # Check if we're at the end of AdminGetPokemon method
    if [[ $IN_ADMIN_METHOD -eq 1 && $FINISHED_SWITCH -eq 1 && $line =~ "}" ]]; then
        IN_ADMIN_METHOD=0
        echo "$line" >> "$TMP_FILE"
        continue
    fi
    
    # If we're in the switch statement of AdminGetPokemon, modify the parameters
    if [[ $IN_ADMIN_METHOD -eq 1 && $FINISHED_SWITCH -eq 0 ]]; then
        # Don't modify the Abra line as it's already correct
        if [[ $line =~ "abra" && $line =~ "new Abra(HP, name, userId, exp)" ]]; then
            echo "$line" >> "$TMP_FILE"
            continue
        fi
        
        # Replace "None", userId! with HP, name, userId, exp for other Pokémon
        if [[ $line =~ "new " && $line =~ "\"None\", userId!" ]]; then
            modified_line=$(echo "$line" | sed 's/"None", userId!/HP, name, userId, exp/g')
            echo "$modified_line" >> "$TMP_FILE"
            continue
        fi
        
        # Check if we've reached the end of the switch statement
        if [[ $line =~ "_ => null" ]]; then
            FINISHED_SWITCH=1
        fi
    fi
    
    # Output the line unchanged if no modifications were made
    echo "$line" >> "$TMP_FILE"
    
done < "$USER_FILE"

# Replace the original file with the modified one
mv "$TMP_FILE" "$USER_FILE"

echo "Updated AdminGetPokemon parameters in $USER_FILE"