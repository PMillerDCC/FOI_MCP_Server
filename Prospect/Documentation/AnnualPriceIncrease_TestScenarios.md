# Annual Price Increase - Manual Test Scenarios

## Prerequisites
- Ensure the application is running and you are logged in
- Have access to accepted contracts in various specification ranges

## Test Scenario 1: Basic Search and Display
**Objective:** Verify that the page loads and searches for accepted contracts in a range

**Steps:**
1. Navigate to Reports > Annual Price Increase from the navigation menu
2. Verify the default values:
   - Specification Start = 1
   - Specification End = 999999
3. Enter a known specification range (e.g., 1 to 100)
4. Click "Search"

**Expected Results:**
- The page displays a grid showing all accepted contracts in the range
- Grid columns include: Contract Ref, Tender Ref, Contractor, Service No, Route, Variation, Current Price/Day, Days, Current Price/Year
- Message displays "Contracts Found: X" where X is the count
- Each row has a checkbox for selection

## Test Scenario 2: No Contracts Found
**Objective:** Verify handling when no accepted contracts exist in the range

**Steps:**
1. Enter a specification range with no accepted contracts (e.g., 99990 to 99999)
2. Click "Search"

**Expected Results:**
- An information message displays: "No accepted contracts found in the specified range"
- No grid is displayed

## Test Scenario 3: Invalid Range
**Objective:** Verify validation for specification range

**Steps:**
1. Enter Specification Start = 500
2. Enter Specification End = 100
3. Click "Search"

**Expected Results:**
- Validation error message: "Specification Start must be less than or equal to Specification End"
- No search is performed

## Test Scenario 4: Calculate Price Increase
**Objective:** Verify that new prices are calculated correctly

**Steps:**
1. Search for a range with accepted contracts
2. Enter a percentage (e.g., 5.0) in the "Price Increase Percentage" field
3. Click "Calculate"

**Expected Results:**
- Two new columns appear in the grid: "New Price/Day" and "New Price/Year"
- For each row, verify that:
  - New Price/Day = Current Price/Day × (1 + Percentage/100)
  - New Price/Year = New Price/Day × Days of Operation
- Values are rounded to 2 decimal places

## Test Scenario 5: Apply Without Selection
**Objective:** Verify that applying requires at least one selection

**Steps:**
1. Search for a range with accepted contracts
2. Enter a percentage increase (e.g., 3.0)
3. Click "Calculate"
4. Do NOT select any checkboxes
5. Click "Apply Price Increase"

**Expected Results:**
- Validation error message: "Please select at least one contract variation to apply the price increase"
- No prices are updated

## Test Scenario 6: Apply Without Percentage
**Objective:** Verify that applying requires a percentage value

**Steps:**
1. Search for a range with accepted contracts
2. Select one or more checkboxes
3. Do NOT enter a percentage
4. Click "Apply Price Increase"

**Expected Results:**
- Validation error message: "Please enter a percentage increase"
- No prices are updated

## Test Scenario 7: Successful Single Contract Update
**Objective:** Verify that a price increase is applied to a single selected contract

**Steps:**
1. Search for a range with accepted contracts
2. Enter a percentage increase (e.g., 2.5)
3. Click "Calculate" to preview new prices
4. Select ONE checkbox
5. Note the contract reference, current prices, and new calculated prices
6. Click "Apply Price Increase"
7. Confirm the application in the confirmation dialog

**Expected Results:**
- Success message: "Price increase applied to 1 contract variation(s)"
- The form resets but retains the specification range
- Navigate to Contracts > [Select the updated contract]
- Verify the latest variation shows:
  - Average Price Per Day = New Price/Day from the calculation
  - Average Price Per Year = New Price/Year from the calculation
- In the variation's "Reason for Edit" field, verify an entry like: "Annual Price Increase: 2.5% applied on [current date]"

## Test Scenario 8: Successful Multiple Contract Update
**Objective:** Verify that a price increase is applied to multiple selected contracts

**Steps:**
1. Search for a range with multiple accepted contracts
2. Enter a percentage increase (e.g., 5.0)
3. Click "Calculate"
4. Select MULTIPLE checkboxes (e.g., 3 or more)
5. Note the selected count display updates (e.g., "Selected: 3 of 10")
6. Click "Apply Price Increase"
7. Confirm the application

**Expected Results:**
- Success message: "Price increase applied to X contract variation(s)" where X matches the selected count
- The form resets
- For each selected contract, verify in the Contracts screen that:
  - The latest variation's prices are updated correctly
  - A "Reason for Edit" entry is added

## Test Scenario 9: Select/Deselect All
**Objective:** Verify the "Select All" checkbox functionality

**Steps:**
1. Search for a range with multiple accepted contracts
2. Click the checkbox in the header row (Select All)
3. Verify all row checkboxes are selected
4. Verify the "Selected: X of Y" count shows all rows selected
5. Click the header checkbox again to deselect
6. Verify all row checkboxboxes are deselected
7. Verify the "Selected: 0 of Y" count

**Expected Results:**
- Select All checkbox correctly toggles all row selections
- The selected count updates dynamically

## Test Scenario 10: Mixed Day Counts
**Objective:** Verify calculations work correctly for contracts with different days of operation

**Steps:**
1. Search for contracts with varying days of operation (e.g., 250, 300, 365)
2. Enter a percentage (e.g., 10.0)
3. Click "Calculate"
4. For each row, manually verify:
   - New Price/Day = Current Price/Day × 1.10
   - New Price/Year = New Price/Day × Days of Operation

**Expected Results:**
- All calculations are correct regardless of days of operation
- Each contract uses its own "Days of Operation" value

## Test Scenario 11: Decimal and Rounding Edge Cases
**Objective:** Verify correct rounding behavior for decimal values

**Steps:**
1. Search for contracts with non-round prices (e.g., £123.45)
2. Enter a percentage that will produce rounding scenarios (e.g., 2.7)
3. Click "Calculate"
4. Verify that:
   - All displayed values are rounded to 2 decimal places
   - New Price/Year is calculated from the rounded New Price/Day

**Expected Results:**
- Prices are rounded correctly using standard rounding rules
- No precision errors or display issues

## Test Scenario 12: Re-Search After Apply
**Objective:** Verify the page can be used for multiple batch updates

**Steps:**
1. Complete a successful apply operation (Scenario 7 or 8)
2. After the success message, enter a NEW specification range
3. Click "Search"
4. Perform another calculation and apply

**Expected Results:**
- The page correctly resets and loads new data
- Previous selections and calculations are cleared
- The new batch update works correctly

## Test Scenario 13: Current Variation Verification
**Objective:** Ensure only the LATEST/CURRENT variation is updated, not historical ones

**Steps:**
1. Find a contract with multiple historical variations (ContractVariationNo > 1)
2. Note the highest ContractVariationNo (this is the current one)
3. Search for that contract in Annual Price Increase
4. Verify the grid shows the variation matching the highest ContractVariationNo
5. Apply a price increase to this contract
6. Navigate to the Contract detail page
7. Verify ONLY the latest variation's prices were updated
8. Verify historical variation prices remain unchanged

**Expected Results:**
- Only the current (highest ContractVariationNo) variation is displayed and updated
- Historical variations are not affected

## Testing Notes
- Test with both small (e.g., 0.5%) and large (e.g., 50%) percentage increases
- Test with both small and large specification ranges
- Verify that the "Reason for Edit" audit trail is consistently applied
- Check behavior with edge cases: exactly 1 contract, exactly 999,999 in spec end, etc.
- Confirm that the navigation menu entry is visible and correctly labeled
- Test canceling the confirmation dialog to ensure no updates occur

## Performance Testing
- Test with a large specification range (e.g., 1 to 99999) to verify reasonable load times
- Monitor database performance during bulk updates (e.g., applying to 50+ contracts)
- Verify the page remains responsive during calculation with many rows
