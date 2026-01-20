// Contact form handler
(function() {
    'use strict';

    // Azure Function URL - Update this with your actual Azure Function endpoint
    //const AZURE_FUNCTION_URL = 'http://localhost:7071/api/contact';
    const AZURE_FUNCTION_URL = 'https://robjohnston-contact-form.azurewebsites.net/api/contact';

    document.addEventListener('DOMContentLoaded', function() {
        const form = document.getElementById('contactForm');
        const successDiv = document.getElementById('contactFormSuccess');
        const errorDiv = document.getElementById('contactFormError');

        if (!form) return;

        form.addEventListener('submit', function(event) {
            event.preventDefault();

            // Get form data
            const fromEmail = document.getElementById('fromEmail').value;
            const message = document.getElementById('message').value;

            // Hide previous messages
            successDiv.style.display = 'none';
            errorDiv.style.display = 'none';

            // Disable submit button to prevent double submission
            const submitBtn = form.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;
            submitBtn.disabled = true;
            submitBtn.textContent = 'Sending...';

            // Prepare form data
            const formData = new URLSearchParams();
            formData.append('fromEmail', fromEmail);
            formData.append('message', message);

            // Submit to Azure Function
            fetch(AZURE_FUNCTION_URL, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: formData.toString()
            })
            .then(function(response) {
                if (!response.ok) {
                    return response.text().then(function(text) {
                        throw new Error(text || 'Failed to send message');
                    });
                }
                return response.text();
            })
            .then(function(data) {
                // Success
                successDiv.textContent = data || 'Thank you! Your message has been sent successfully.';
                successDiv.style.display = 'block';
                form.reset();
            })
            .catch(function(error) {
                // Error
                errorDiv.textContent = 'Error: ' + error.message + '. Please try again or contact me directly via social media.';
                errorDiv.style.display = 'block';
            })
            .finally(function() {
                // Re-enable submit button
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;
            });
        });
    });
})();
