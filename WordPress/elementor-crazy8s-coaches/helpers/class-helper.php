<?php
/**
 * Helper class.
 *
 * @category   Class
 * @package    ElementorCrazy8sCoaches
 * @subpackage WordPress
 * @author     Dug Steen <dug@softcrow.net>
 * @copyright  2025 Dug Steen
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @since      1.0.0
 * php version 8.1
 */

namespace ElementorCrazy8sCoaches\Helpers;

// Security Note: Blocks direct access to the plugin PHP files.
defined( 'ABSPATH' ) || die();

/**
 * Class Helper
 *
 * Helper class for various utility functions.
 *
 * @since 1.0.0
 */
class Helper {

    /**
     * Fetches order data based on settings
     *
     * @since 1.0.0
     * @access public
     *
     * @return string
     */
	
    public function fetch_order_data($settings) {
        // Set the base URL based on the platform
        switch ($settings['platform']) {
            case 'development':
                $base_url = "https://localhost:7071/api/";
                break;
            case 'staging':
                $base_url = "https://c8s-functions-staging.azurewebsites.net/api/";
                break;
            case 'production':
            default:
                $base_url = "https://api.crazy8sclub.org/api/";
                break;
        }

        $user_id = get_current_user_id();
        $api_url = $base_url . "coach/?id=" . $user_id;

        $args = [
            'headers' => [
                'x-functions-key' => $settings['api-key']
            ]
        ];
        $response = wp_remote_get($api_url, $args);
        $status_code = wp_remote_retrieve_response_code($response);

        if (is_wp_error($response)) {
            return (object)[
                'error_type' => 'NETWORK ERROR',
                'error_message' => $response->get_error_message()
            ];
        } else if ($status_code != 200) {
            return (object)[
                'error_type' => 'API ERROR',
                'error_message' => "Bad status code returned: [$status_code]"
            ];
        }

        $body = wp_remote_retrieve_body($response);
        $data = json_decode($body, true);
        if (isset($data['Exception'])) {
            return (object)[
                'error_type' => 'RESPONSE ERROR',
                'error_message' => $data['Exception']['message']
            ];
        } else if (!isset($data['Result'])) {
            return (object)[
                'error_type' => 'NO DATA',
                'error_message' => 'The current user has no associated Orders or SKUs.'
            ];
        }

        return $data;
    }
}