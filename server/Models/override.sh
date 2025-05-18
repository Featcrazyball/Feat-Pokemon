#!/bin/bash

# Directory containing Pokémon class files
POKEMON_DIR="c:/Users/yangx/OneDrive/Desktop/Feat Pokemon/Feat-Pokemon/server/Models/Pokemons"

processed=0
modified=0

echo "Beginning to process Pokémon classes..."

# Process each Pokémon class file
for file in "$POKEMON_DIR"/*.cs; do
    filename=$(basename "$file")
    processed=$((processed+1))
    
    # Skip if GodEvolve already exists
    if grep -q "public override async Task GodEvolve" "$file"; then
        echo "Skipping $filename - GodEvolve already exists"
        continue
    fi

    echo "Processing $filename..."

    # Find the last closing brace
    insert_line=$(grep -n "^}" "$file" | tail -1 | cut -d: -f1)
    
    # For final evolution Pokémon
    if grep -q "is already at its final evolution stage" "$file"; then
        # Create a temporary file for the GodEvolve method
        cat > temp_godevolve.txt << EOF
    public override async Task GodEvolve(ClientSession session)
    {
        await session.SendMessageAsync(\$"{(Nickname == "None" ? Name : Nickname)} is already at its final evolution stage.");
    }

EOF
        # Insert before the last closing brace
        sed -i "${insert_line}i\\$(cat temp_godevolve.txt)" "$file"
        rm temp_godevolve.txt
        
        echo "Added final-stage GodEvolve to $filename"
        modified=$((modified+1))
    
    # For evolvable Pokémon
    elif grep -q "public override async Task Evolve" "$file" && grep -q "EvolvesTo" "$file"; then
        # Extract pokemon name and evolution target
        pokemon_name=$(grep "class" "$file" | head -1 | awk '{print $2}')
        evolves_to=$(grep "EvolvesTo" "$file" | grep -o '"[^"]*"' | head -1 | tr -d '"')
        
        if [ -n "$evolves_to" ]; then
            # Create a temporary file for the GodEvolve method
            cat > temp_godevolve.txt << EOF
    public override async Task GodEvolve(ClientSession session)
    {
        using (var context = new DatabaseContext())
        {
            var evolved = new ${evolves_to}(this);
            evolved.MaxHealth = evolved.HealthOverride;
            evolved.EvolveLevelUp(Level-1);

            foreach (var skill in this.Skills)
            {
                context.Skills.Remove(skill);
            }

            context.PokemonMaster.Remove(this);
            context.PokemonMaster.Add(evolved);
            
            foreach (var skill in evolved.Skills)
            {
                context.Skills.Add(skill);
            }
            
            context.SaveChanges();
        }
        await session.SendMessageAsync(\$"{(Nickname == "None" ? Name : Nickname)} has evolved from a ${pokemon_name} to a ${evolves_to}!");
    }

EOF
            # Insert before the last closing brace
            sed -i "${insert_line}i\\$(cat temp_godevolve.txt)" "$file"
            rm temp_godevolve.txt
            
            echo "Added evolution GodEvolve to $filename"
            modified=$((modified+1))
        fi
    fi
done

echo "Script complete: Processed $processed files, modified $modified files"