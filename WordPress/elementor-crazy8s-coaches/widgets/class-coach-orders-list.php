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
use ElementorCrazy8sCoaches\Helpers\Helper;

// Security Note: Blocks direct access to the plugin PHP files.
defined('ABSPATH') || die();

/**
 * CoachOrdersList widget class.
 *
 * @since 1.0.0
 */
class CoachOrdersList extends Widget_Base
{
	const FAKE_ORDER = array(
		'number' => '12345',
		'status' => 'Processing',
		'name' => 'Edna Crabapple',
		'recipient' => 'Springfield Middle School',
		'shipping_address_1' => '123 Main St',
		'shipping_address_2' => 'Apt 4B',
		'city' => 'Springfield',
		'state' => 'OH',
		'zip_code' => '30521',
		'ordered_on' => '2025-01-10T21:35:13.9776377+00:00',
		'shipped_on' => '2025-01-15T05:20:31.0017605+00:00',
	);
	private $helper;

	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct($data = array(), $args = null)
	{
		parent::__construct($data, $args);

		$this->helper = new Helper();

		//		wp_register_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES), array(), '1.0.1');
		wp_register_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES) . '?v=' . time(), array(), null);
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
		return 'coach-orders-list';
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
		return __('Coach Orders List', 'elementor-crazy8s-coaches');
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
		return 'fa fa-boxes-packing';
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
		$settings = $this->get_settings_for_display();

		echo '<div class="c8s-coach-orders-list">';


		$orders = [];
		if (\Elementor\Plugin::$instance->editor->is_edit_mode()) {
			$orders[] = self::FAKE_ORDER;
		} else {
			$result = $this->helper->fetch_order_data($settings);

			// Check the result and handle accordingly
			if (is_object($result) && isset($result->error_type) && isset($result->error_message)) {
				// Handle error
				echo '<div class="c8s-text-danger">';
				echo '<strong>' . $result->error_type . '</strong>: ' . $result->error_message;
				echo '</div>';
			} else if (is_array($result)) {

				$data = $result['Result'];
				if (isset($data['clubs'])) {
					foreach ($data['clubs'] as $club) {
						if (isset($club['orders'])) {
							foreach ($club['orders'] as $order) {
								$orders[] = $order;
							}
						}
					}
				}

				// Sort the orders array in reverse-chronological order
				usort($orders, function ($a, $b) {
					return strtotime($b['ordered_on']) - strtotime($a['ordered_on']);
				});

				foreach ($orders as $order) {
					$this->render_order($order);
				}
			}
		}

		echo '</div>';
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
		echo '<div><strong>Platform:</strong> ' . get_option('crazy8s_platform') . '</div>';
		echo '<div><strong>API Key:</strong> ' . get_option('crazy8s_api_key') . '</div>';

		// Enqueue the plugin stylesheet
		wp_enqueue_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES) . '?v=' . time(), array(), null);

		$this->render_order(self::FAKE_ORDER);
	}

	/**
	 * Render a single order
	 *
	 * @since 1.0.0
	 *
	 * @access private
	 */
	private function render_order($order)
	{
		$order_number = $order['number'];
		$order_status = $order['status'];
		$order_name = $order['contact_name'];
		$order_recipient = $order['recipient'];
		$order_address1 = $order['shipping_address_1'];
		$order_address2 = $order['shipping_address_2'];
		$order_city_state_zip = $order['city'] . ", " . $order['state'] . " " . $order['zip_code'];

		// Convert ordered_on and shipped_on to Eastern Time Zone and format as month/day/year
		$eastern_time_zone = new \DateTimeZone('America/New_York');
		$ordered_on = new \DateTime($order['ordered_on']);
		$ordered_on->setTimezone($eastern_time_zone);
		$ordered_on_formatted = $ordered_on->format('m/d/Y');

		$shipped_on = new \DateTime($order['shipped_on']);
		$shipped_on->setTimezone($eastern_time_zone);
		$shipped_on_formatted = $shipped_on->format('m/d/Y');

?>
		<div class="c8s-coach-order-container">
			<div class="c8s-coach-order-subcontainer">
				<div class="c8s-coach-order-row">
					<span class="c8s-coach-order-caption">Order #:</span>
					<?php echo $order_number ?>
				</div>
				<div class="c8s-coach-order-row">
					<span class="c8s-coach-order-caption">Status:</span>
					<?php echo $order_status ?>
				</div>
			</div>
			<div class="c8s-coach-order-subcontainer">
				<div class="c8s-coach-order-caption">
					Sent To:
				</div>
				<div class="c8s-coach-order-row">
					<?php echo $order_name ?>
				</div>
				<div class="c8s-coach-order-row">
					<?php echo $order_recipient ?>
				</div>
				<div class="c8s-coach-order-row">
					<?php echo $order_address1 ?>
				</div>
				<?php if (isset($order_address2)) { ?>
					<div class="c8s-coach-order-row">
						<?php echo $order_address2 ?>
					</div>
				<?php } ?>
				<div class="c8s-coach-order-row">
					<?php echo $order_city_state_zip ?>
				</div>
			</div>
			<div class="c8s-coach-order-subcontainer">
				<div class="c8s-coach-order-row">
					<span class="c8s-coach-order-caption">Ordered:</span>
					<?php echo $ordered_on_formatted ?>
				</div>
				<div class="c8s-coach-order-row">
					<span class="c8s-coach-order-caption">Shipped:</span>
					<?php echo $shipped_on_formatted ?>
				</div>
				<?php
				if (isset($order['shipments'])) {
				?>
					<div class="c8s-coach-order-row">
						<div class="c8s-coach-order-caption">Tracking:</div>
						<?php
						$tracking_numbers = [];
						foreach ($order['shipments'] as $shipment) {

							$tracking_number = $shipment['tracking_number'];
							$ship_method = ($shipment['ship_method'] !== 'Other') ? $shipment['ship_method'] : $shipment['ship_method_other'];

							if (in_array($tracking_number, $tracking_numbers)) { continue; }

							echo '<div class="c8s-coach-order-tracking">';
							echo $ship_method;
							echo ': ';

							if (isset($tracking_number)) {
								if ($ship_method == 'FedEx') {
									echo '<a href="https://www.fedex.com/fedextrack/?trknbr=' . $tracking_number . '" target="_fedex">';
									echo $tracking_number;
									echo '</a>';
								}
								else {
									echo $tracking_number;
								}
							} else {
								echo '(not set)';
							}
							echo '</div>';

							$tracking_numbers[] = $tracking_number;
						}
						?>

					</div>
				<?php
				}
				?>
			</div>
		</div>
<?php
	}
}
