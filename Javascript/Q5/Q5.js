const Holidays = require('date-holidays');

function getSingaporeWorkingDays(date, additionalHolidays = []) {
	// Validate date format
    if (!(date instanceof Date) || isNaN(date)) {
        throw new Error('Invalid date parameter');
    }

    const year = date.getFullYear();
    const month = date.getMonth();
    
    // Get official Singapore holidays (preserve original format)
    const hd = new Holidays('SG');
    const officialHolidays = hd.getHolidays(year)
        .filter(holiday => {
            /**
			 * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/Date
			 * JavaScript's Date constructor expects ISO 8601 format strings, 
			 * which use 'T' between date and time
			 * 
			 * With 'Z', it's  treated as UTC (Coordinated Universal Time)
			 */
            const holidayDate = new Date(holiday.date.replace(' ', 'T') + 'Z');
            return holidayDate.getUTCMonth() === month;
        });

    // 2. Process additional holidays (keep as Date objects)
    const customHolidays = additionalHolidays.map(d => 
        typeof d === 'string' ? new Date(d) : d
    );

    // 3. Calculate working days
    return calculateWorkingDays(date, officialHolidays, customHolidays);
}

function calculateWorkingDays(date, officialHolidays, customHolidays = []) {
	/**
	 * 0 params mean the day before the first day,
	 * like the previous day of May 1 is April 30
	 */
    const lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

	// Show official holiday
	console.log("Official Holiday dates:")
	console.log(officialHolidays);

	// Show custom holiday
	console.log("Custom Holiday dates:")
	console.log(customHolidays);
    
    // Create sets for holiday dates
    const officialHolidayDates = new Set(
        officialHolidays.map(h => new Date(h.date.replace(' ', 'T') + 'Z').getUTCDate())
    );
    
    const customHolidayDates = new Set(
        customHolidays.map(d => d.getUTCDate())
    );

    let workingDays = 0;
    let nonWorkingDays = 0;
    
    for (let day = 1; day <= lastDay.getDate(); day++) {
        const currentDate = new Date(date.getFullYear(), date.getMonth(), day);
		/**
		 * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/getDay
		 * Sunday - Saturday : 0 - 6
		 */
        const isWeekend = currentDate.getDay() === 0 || currentDate.getDay() === 6;
        const isHoliday = officialHolidayDates.has(day) || customHolidayDates.has(day);
        
        if (isWeekend || isHoliday) {
            nonWorkingDays++;
        } else {
            workingDays++;
        }
    }
    
    return {
        workingDays,
        nonWorkingDays,
        totalDays: workingDays + nonWorkingDays,
        holidays: [
            ...officialHolidays,
            ...customHolidays.map(d => ({
                date: d,
                name: 'Custom Holiday'
            }))
        ]
    };
}

(async () => {
    try {
        const date = new Date('2023-05-01');

		const customHolidays = [
            '2023-05-10',
            // new Date('2023-05-17'), // Date object
            // '2023-05-25T00:00:00Z' // ISO string with time
        ];

        const result = getSingaporeWorkingDays(date,customHolidays);
        
        console.log('\nMay 2023 Results:');
        console.log(`Working Days: ${result.workingDays}`);
        console.log(`Non-Working Days: ${result.nonWorkingDays}`);
        console.log('Holidays:', result.holidays);
    } catch (error) {
        console.error('Error:', error.message);
    }
})();