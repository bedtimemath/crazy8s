<?php

/**
 * Coach Skus Dropdown class.
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

namespace ElementorCrazy8sCoaches\Widgets;

use Elementor\Widget_Base;
use Elementor\Controls_Manager;

// Security Note: Blocks direct access to the plugin PHP files.
defined('ABSPATH') || die();

/**
 * CoachSkusDropdown widget class.
 *
 * @since 1.0.0
 */
class CoachSkusDropdown extends Widget_Base
{
	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct($data = array(), $args = null)
	{
		parent::__construct($data, $args);

		wp_register_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES), array(), '1.0.0');
	}

	/**
	 * Retrieve the widget name.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget name.
	 */
	public function get_name()
	{
		return 'coach-skus-dropdown';
	}

	/**
	 * Retrieve the widget title.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget title.
	 */
	public function get_title()
	{
		return __('Coach Skus Dropdown', 'elementor-crazy8s-coaches');
	}

	/**
	 * Retrieve the widget icon.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget icon.
	 */
	public function get_icon()
	{
		return 'fa fa-list-dropdown';
	}

	/**
	 * Retrieve the list of categories the widget belongs to.
	 *
	 * Used to determine where to display the widget in the editor.
	 *
	 * Note that currently Elementor supports only one category.
	 * When multiple categories passed, Elementor uses the first one.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return array Widget categories.
	 */
	public function get_categories()
	{
		return array('crazy8s');
	}

	/**
	 * Enqueue styles.
	 */
	public function get_style_depends()
	{
		return array('crazy8s-elementor');
	}

	/**
	 * Register the widget controls.
	 *
	 * Adds different input fields to allow the user to change and customize the widget settings.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _register_controls()
	{
		$this->start_controls_section(
			'section_content',
			array(
				'label' => __('Content', 'elementor-crazy8s-coaches'),
			)
		);

		$this->add_control(
			'platform',
			array(
				'label' => __('Platform', 'elementor-crazy8s-coaches'),
				'type'	=> \Elementor\Controls_Manager::SELECT,
				'default' => 'development',
				'options' => [
					'development' => __('Development', 'development'),
					'staging' => __('Staging', 'staging'),
					'production' => __('Production', 'production'),
				]
			)
		);

		$this->add_control(
			'api-key',
			array(
				'label' => __('API Key', 'elementor-crazy8s-coaches'),
				'label_block' => true,
				'type'	=> \Elementor\Controls_Manager::TEXT,
				'placeholder' => 'Enter the Azure functions key here'
			)
		);

		$this->end_controls_section();
	}

	/**
	 * Render the widget output on the frontend.
	 *
	 * Written in PHP and used to generate the final HTML.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function render()
	{
?>
		<div class="c8s-coach-skus-dropdown">
			<?php
			$settings = $this->get_settings_for_display();

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
				$error_code = $response->get_error_code();
				$error_message = $response->get_error_message();
			?>
				<div class="c8s-text-danger">
					<strong>NETWORK ERROR</strong>:
					<div>Code = <?php echo $error_code; ?></div>
					<div><?php echo $error_message; ?></div>
				</div>
			<?php
				return;
			} else if ($status_code != 200) {
			?>
				<div class="c8s-text-danger">
					<strong>API ERROR</strong>:
					[<?php echo $status_code; ?>] Bad status code returned.
				</div>
			<?php
			}

			$body = wp_remote_retrieve_body($response);
			$data = json_decode($body, true);
			if (isset($data['Exception'])) {
			?>
				<div class="c8s-text-danger">
					<strong>RESPONSE ERROR</strong>:
					[<?php echo $data['Exception']['source']; ?>] <?php echo $data['Exception']['message'] ?>
				</div>
			<?php
				return;
			} else if (!isset($data['Result'])) {
			?>
				<div class="c8s-text-danger">
					The current user has no associated Orders or SKUs.
				</div>
			<?php
				return;
			}


			$skus = [];

			if (isset($data['Result']['clubs'])) {
				foreach ($data['Result']['clubs'] as $club) {
					if (isset($club['orders'])) {
						foreach ($club['orders'] as $order) {
							if (isset($order['order_skus'])) {
								foreach ($order['order_skus'] as $order_sku) {
									if (isset($order_sku['sku'])) {
										$skus[] = $order_sku['sku'];
									}
								}
							}
						}
					}
				}
			}
			?>
				<select name="skus_dropdown">
					<?php foreach ($skus as $sku) { ?>
						<option value="<?php echo $sku['key']; ?>"><?php echo $sku['name']; ?></option>
					<?php } ?>
				</select>
		</div>
	<?php
	}

	/**
	 * Render the widget output in the editor.
	 *
	 * Written as a Backbone JavaScript template and used to generate the live preview.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _content_template()
	{
	?>
		<div>Skus Dropdown Goes Here</div>
		<div>Platform: {{{ settings.platform }}}</div>
<?php
	}
}
