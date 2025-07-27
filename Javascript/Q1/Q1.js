function calMaxSubArray(arr) {
    if (arr.length === 0) return { sum: 0, subarray: [] };
    
    // Define variable
    let maxCurrent = arr[0];
    let maxGlobal = arr[0];
    let currentStart = 0;
    let globalStart = 0;
    let globalEnd = 0;
    
    // Array to store each step's information
    const steps = [];
    
    // Record initial state
    steps.push({
        iteration: 0,
        currentValue: arr[0],
        maxCurrent: maxCurrent,
        currentSubarray: arr.slice(currentStart, 1),
        maxGlobal: maxGlobal,
        maxSubarray: arr.slice(globalStart, globalEnd + 1),
        decision: "Initial setup"
    });
    
    // For loop whole array
    for (let i = 1; i < arr.length; i++) {
        let decision;

        /**
         * If current number > (max current + current number),
         * meaning that this number is bigger than upfront all,
         * we should start get new subarray position from this one
         * Like if -2 > -6 + -2, we should take -2 as our new subarray position
         */
        if (arr[i] > maxCurrent + arr[i]) {
            decision = `Start new subarray at index ${i} (${arr[i]} > ${maxCurrent + arr[i]})`;
            /**
             * Current max current number will only be reset
             * when current number is bigger max current + current number
             */
            maxCurrent = arr[i];
            /**
             * Current start position will only be reset
             * when current number is bigger max current + current number
             */
            currentStart = i;
        } else {
            /**
             * If current number is small max current + current number,
             * mean that the continuous sum is bigger,
             * we should keep it.
             */
            decision = `Extend subarray with index ${i} (${arr[i]} ≤ ${maxCurrent + arr[i]})`;
            maxCurrent += arr[i];
        }
        
        let globalChanged = false;

        /**
         * If max current bigger than max global,
         * mean that we should update our subarray with current which get the bigest
         */
        if (maxCurrent > maxGlobal) {
            globalChanged = true;
            maxGlobal = maxCurrent;

            /**
             * Update subarray position
             */
            globalStart = currentStart;
            globalEnd = i;
        }
        
        // Record this step
        steps.push({
            iteration: i,
            currentValue: arr[i],
            maxCurrent: maxCurrent,
            currentSubarray: arr.slice(currentStart, i + 1),
            maxGlobal: maxGlobal,
            maxSubarray: arr.slice(globalStart, globalEnd + 1),
            decision: decision,
            globalChanged: globalChanged
        });
    }
    
    const result = {
        sum: maxGlobal,
        array: arr,
        maxSubarray: arr.slice(globalStart, globalEnd + 1),
        steps: steps
    };
    
    return result;
}

// Helper function to display the steps nicely
function displaySteps(result) {
    console.log("\nProcessing Steps:");
    result.steps.forEach(step => {
        console.log(`\nIteration ${step.iteration}:`);
        console.log(`- Current value: ${step.currentValue}`);
        console.log(`- Decision: ${step.decision}`);
        console.log(`- Current subarray: [${step.currentSubarray.join(', ')}] (sum: ${step.maxCurrent})`);
        console.log(`- Max subarray so far: [${step.maxSubarray.join(', ')}] (sum: ${step.maxGlobal})`);
        if (step.globalChanged) {
            console.log("  ^ New global maximum found!");
        }
    });
    
    console.log("\nFinal Result:");
    console.log(`- Original array: [${result.array.join(', ')}]`);
    console.log(`- Maximum subarray: [${result.maxSubarray.join(', ')}]`);
    console.log(`- Maximum sum: ${result.sum}`);
}

// Test cases
// console.log("\nExample 1:");
// const result1 = calMaxSubArray([1, 2, 3, -2, 5]);
// displaySteps(result1);

console.log("\nExample 2:");
const result2 = calMaxSubArray([-2, 1, -3, 4, -1, 2, 1, -5, 4]);
displaySteps(result2);

// console.log("\nExample 3:");
// const result3 = calMaxSubArray([-1, -2, -3]);
// displaySteps(result3);